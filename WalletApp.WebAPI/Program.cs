using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Castle.DynamicProxy;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.Abstraction.Services;
using WalletApp.Application.Abstraction.Services.CurrentUserServices;
using WalletApp.Application.Abstraction.Services.IoC;
using WalletApp.Application.Abstraction.Services.MailServices;
using WalletApp.Application.Abstraction.Services.Notification;
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
    .ReadFrom.Configuration((Microsoft.Extensions.Configuration.IConfiguration)configuration, sectionName: "LoggingConfig:Providers:Serilog")
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

// E-posta & diğer servisler
// HttpContext & CurrentUser
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
// E-posta & diğer servisler
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, SmtpEmailService>();
builder.Services.AddApplicationServices();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommandHandler).Assembly));
builder.Services.AddControllersWithViews().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Program>());


builder.Services.AddDbContext<WalletDbContext>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddSingleton<ProxyGenerator>();


// Authentication & Authorization
builder.Services.AddJwtService(configuration);
builder.Services.AddAuthorization();

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenRegistration();


// AOP - Transaction
builder.Services.AddTransactionServices();
// TransferCommandHandler register (MediatR otomatik çalışacak)
builder.Services.AddTransient<TransferCommandHandler>();

// Banka Servisleri 
builder.Services.AddScoped<IBankServicesFactory, BankServicesFactory>();
builder.Services.AddScoped<VakifBankServices>(provider =>
    new VakifBankServices(provider.GetRequiredService<IProviderBankRepository>()));

builder.Services.AddScoped<ZiraatBankServices>(provider =>
    new ZiraatBankServices(provider.GetRequiredService<IProviderBankRepository>()));

builder.Services.AddScoped<GarantiBankServices>(provider =>
    new GarantiBankServices(provider.GetRequiredService<IProviderBankRepository>()));

builder.Services.AddSignalR();
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

app.MapHub<NotificationHub>("");

app.Run();
