using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.Abstraction.Services;
using WalletApp.Application.Feature.BankAccount.Validations;
using WalletApp.Application.Feature.Wallet.Handlers;
using WalletApp.Domain.Entities;
using WalletApp.Domain.Enums;
using WalletApp.Infrastructure.Repositories;
using WalletApp.Infrastructure.Services.EmailServices;
using WalletApp.Persistence.Base;
using WalletApp.Persistence.Context;
using WalletApp.Persistence.Repositories;
using WalletApp.WebAPI.Middleware;


var builder = WebApplication.CreateBuilder(args);

// --- SERILOG CONFIGURATION ---

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var columnOptions = new ColumnOptions();
columnOptions.Store.Remove(StandardColumn.Properties); // json props’u kaldırır
columnOptions.Store.Add(StandardColumn.LogEvent);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.MSSqlServer(
        connectionString: connectionString,
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "Logs",
            AutoCreateSqlTable = true
        },
        columnOptions: columnOptions)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();


// Kestrel IP ayarı
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Listen(System.Net.IPAddress.Any, 5000);
});

// CORS politikası
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// DbContext
builder.Services.AddDbContext<WalletDbContext>(options =>
    options.UseSqlServer(connectionString));

// HttpContextAccessor ve CurrentUserService
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// JWT Authentication ayarları
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

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

// Controllers
builder.Services.AddControllers();

// Swagger ayarları
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
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] { }
        }
    });
});

// Email ve diğer servisler
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, SmtpEmailService>();
builder.Services.AddScoped<WalletService>();

// Repositories & Services
builder.Services.AddScoped<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IUserDetailRepository, UserDetailRepository>();
builder.Services.AddScoped<IWalletTransferRepository, WalletTransferRepository>();
builder.Services.AddScoped<IBankTransactionRepository, BankTransactionRepository>();
builder.Services.AddScoped<IProviderBankRepository, ProviderBankRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IBankRouteRepository, BankRouteRepository>();
builder.Services.AddScoped(typeof(IEntityRepository<>), typeof(EfEntityRepositoryBase<>));

// MediatR + FluentValidation
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommandHandler).Assembly));

builder.Services.AddValidatorsFromAssemblyContaining<CreateBankAccountRequestValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddValidatorsFromAssemblyContaining<BankTransferRequestValidator>();

// MemoryCache
builder.Services.AddMemoryCache();

var app = builder.Build();

// Admin user seed (örnek)
await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<WalletDbContext>();
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<AppUser>>();

    var email = config["AdminUser:Email"];
    var password = config["AdminUser:Password"];
    var username = config["AdminUser:UserName"];

    var adminExists = await dbContext.Users.AnyAsync(u => u.Email == email && u.Role == UserRole.Admin);

    if (!adminExists)
    {
        var admin = new AppUser
        {
            Email = email,
            Role = UserRole.Admin,
            PasswordHash = passwordHasher.HashPassword(null, password),
            CreatedDate = DateTime.UtcNow
        };

        dbContext.Users.Add(admin);
        await dbContext.SaveChangesAsync();
    }
}

// Middleware pipeline
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
app.UseMiddleware<AppUserMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

// Burada Serilog loglama middleware’ini çağırıyoruz:
app.UseMiddleware<RequestLoggingMiddleware>();

app.MapControllers();

app.Run();
