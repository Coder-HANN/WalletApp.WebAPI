using Autofac;
using Autofac.Extensions.DependencyInjection;
using Castle.DynamicProxy;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Transactions;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.Abstraction.Services;
using WalletApp.Application.Abstraction.Services.CurrentUserServices;
using WalletApp.Application.Abstraction.Services.IoC;
using WalletApp.Application.Abstraction.Services.MailServices;
using WalletApp.Application.Abstraction.Services.Transaction;
using WalletApp.Application.Common;
using WalletApp.Application.Feature.Wallet.Handlers;
using WalletApp.Domain.Entities;
using WalletApp.Domain.Enums;
using WalletApp.Infrastructure.Logging;
using WalletApp.Infrastructure.Services.BankServices;
using WalletApp.Infrastructure.Services.EmailServices;
using WalletApp.Infrastructure.Services.MemoryCach;
using WalletApp.Persistence.Context;
using WalletApp.Persistence.Extensions;
using WalletApp.WebAPI.Middleware;
//using WalletApp.Application.Abstraction.Services.Redis;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// ---------- Serilog (singleton) ----------
var serilogConfigSection = configuration.GetSection("LoggingConfig:Providers:Serilog");

// Log seviyeleri, enrich vs. JSON’dan; sink ve kolonlar koddan:
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration, sectionName: "LoggingConfig:Providers:Serilog")
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .WriteTo.Console()
    .WriteTo.MSSqlServer(
        connectionString: configuration.GetConnectionString("DefaultConnection"),
        tableName: "Logs",
        autoCreateSqlTable: true,
        columnOptions: LoggingColumns.GetColumnOptions()
    )
    .CreateLogger();

builder.Services.AddSingleton<Serilog.ILogger>(logger);

// ---------- DI ----------
builder.Services.AddScoped<SerilogLogger>();                // concrete
builder.Services.AddScoped<ILogService, CompositeLogger>(); // ILogService sadece Composite

builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(System.Net.IPAddress.Any, 5000);
});

// Memory Cache
builder.Services.AddMemoryCache();
// Container - Autofac yapısı araştır
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    // Memory Cache
    containerBuilder.RegisterType<MemoryCacheManager>()
                    .As<ICacheManager>()
                    .SingleInstance();
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});

// DbContext
var connectionString = configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<WalletDbContext>(options => options.UseSqlServer(connectionString));

// HttpContext & CurrentUser
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// JWT
var jwtSettings = configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        RoleClaimType = ClaimTypes.Role
    };
});

builder.Services.AddAuthorization();

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("Admin", new OpenApiInfo { Title = "Admin API", Version = "v1" });
    options.SwaggerDoc("Public", new OpenApiInfo { Title = "Public API", Version = "v1" });

    options.DocInclusionPredicate((group, api) =>
    {
        if (!api.TryGetMethodInfo(out var methodInfo)) return false;
        var attr = methodInfo.DeclaringType?.GetCustomAttribute<ApiExplorerSettingsAttribute>();
        return attr?.GroupName == group;
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Bearer token. Örn: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }}, Array.Empty<string>() }
    });
});

// E-posta & diğer servisler
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, SmtpEmailService>();
builder.Services.AddApplicationServices();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommandHandler).Assembly));
builder.Services.AddControllersWithViews().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Program>());


builder.Services.AddDbContext<WalletDbContext>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddSingleton<ProxyGenerator>();


// AOP - Transaction
builder.Services.AddTransient<WalletService>(provider =>
{
    var transactionService = provider.GetRequiredService<ITransactionService>();
    var proxyGenerator = provider.GetRequiredService<ProxyGenerator>();

    var walletRepository = provider.GetRequiredService<IWalletRepository>();
    var transactionRepository = provider.GetRequiredService<ITransactionRepository>();
    var walletTransferRepository = provider.GetRequiredService<IWalletTransferRepository>();
    var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
    var providerBankRepository = provider.GetRequiredService<IProviderBankRepository>();
    var bankTransactionRepository = provider.GetRequiredService<IBankTransactionRepository>();
    var currentUserService = provider.GetRequiredService<ICurrentUserService>();

    var walletService = new WalletService(
        walletRepository,
        transactionRepository,
        walletTransferRepository,
        httpContextAccessor,
        providerBankRepository,
        bankTransactionRepository,
        currentUserService
    );

    return proxyGenerator.CreateClassProxyWithTarget(
        walletService,
        new TransactionAspect(transactionService)
    );
});

// TransferCommandHandler register (MediatR otomatik çalışacak)
builder.Services.AddTransient<TransferCommandHandler>();

builder.Services.AddScoped<IBankServicesFactory, BankServicesFactory>();
builder.Services.AddScoped<VakifBankServices>(provider =>
    new VakifBankServices(provider.GetRequiredService<IProviderBankRepository>()));

builder.Services.AddScoped<ZiraatBankServices>(provider =>
    new ZiraatBankServices(provider.GetRequiredService<IProviderBankRepository>()));

builder.Services.AddScoped<GarantiBankServices>(provider =>
    new GarantiBankServices(provider.GetRequiredService<IProviderBankRepository>()));

ServiceTool.Create(builder.Services);

var app = builder.Build();

// Admin seed
await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<WalletDbContext>();
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var passwordHasher = scope.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.IPasswordHasher<AppUser>>();

    var email = config["AdminUser:Email"];
    var password = config["AdminUser:Password"];
    var username = config["AdminUser:UserName"];

    if (!await dbContext.Users.AnyAsync(u => u.Email == email && u.Role == UserRole.Admin))
    {
        dbContext.Users.Add(new AppUser
        {
            Email = email!,
            Role = UserRole.Admin,
            PasswordHash = passwordHasher.HashPassword(null!, password!),
            CreatedDate = DateTime.UtcNow
        });
        await dbContext.SaveChangesAsync();
    }
}

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/Admin/swagger.json", "Admin API");
        options.SwaggerEndpoint("/swagger/Public/swagger.json", "Public API");
    });
}

app.UseCors();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<AppUserMiddleware>();
app.UseMiddleware<RequestResponseLoggingMiddleware>();

app.MapControllers();

app.Run();
