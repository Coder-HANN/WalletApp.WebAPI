using FluentValidation.AspNetCore;
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
using WalletApp.Application.Abstraction.Services;
using WalletApp.Application.Common;
using WalletApp.Domain.Entities;
using WalletApp.Domain.Enums;
using WalletApp.Infrastructure.Services.EmailServices;
using WalletApp.Persistence.Context;
using WalletApp.Persistence.Extensions;
using WalletApp.WebAPI.Middleware;

// ----------------- Builder -----------------
var builder = WebApplication.CreateBuilder(args);

// 1️⃣ JSON'dan Serilog ayarlarını oku
var serilogConfig = builder.Configuration.GetSection("LoggingConfig:Providers:Serilog");
var columnOptionsSection = serilogConfig.GetSection("WriteTo:1:Args:columnOptionsSection");
var columnOptions = new ColumnOptions();
columnOptionsSection.Bind(columnOptions);

// 2️⃣ Serilog config
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = serilogConfig.GetValue<string>("WriteTo:1:Args:tableName"),
            AutoCreateSqlTable = serilogConfig.GetValue<bool>("WriteTo:1:Args:autoCreateSqlTable")
        },
        columnOptions: columnOptions,
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug
    )
    .CreateLogger();

Log.Information("Test log to DB");

// ----------------- Services -----------------
builder.Logging.ClearProviders();
builder.Host.UseSerilog();

// Logging servislerini DI'a ekle
builder.Services.AddSingleton<SerilogLogger>();
builder.Services.AddSingleton<ILogService, CompositeLogger>();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(System.Net.IPAddress.Any, 5000);
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<WalletDbContext>(options =>
    options.UseSqlServer(connectionString));

// HttpContext ve CurrentUser
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// JWT Auth
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
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] { }
        }
    });
});

// Email & diğer servisler
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, SmtpEmailService>();
builder.Services.AddApplicationServices();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommandHandler).Assembly));
builder.Services.AddControllersWithViews().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Program>());
builder.Services.AddMemoryCache();

// ----------------- App -----------------
var app = builder.Build();

// Admin seed
await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<WalletDbContext>();
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<AppUser>>();

    var email = config["AdminUser:Email"];
    var password = config["AdminUser:Password"];
    var username = config["AdminUser:UserName"];

    if (!await dbContext.Users.AnyAsync(u => u.Email == email && u.Role == UserRole.Admin))
    {
        dbContext.Users.Add(new AppUser
        {
            Email = email,
            Role = UserRole.Admin,
            PasswordHash = passwordHasher.HashPassword(null, password),
            CreatedDate = DateTime.UtcNow
        });
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

// ✅ Logging middleware en sona değil, auth'dan önce
app.UseMiddleware<RequestResponseLoggingMiddleware>();

app.MapControllers();
app.Run();
