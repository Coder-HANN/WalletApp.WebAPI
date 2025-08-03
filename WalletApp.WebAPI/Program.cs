using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;
using WalletApp.Application.Feature.BankAccount.Validations;
using WalletApp.Application.Feature.Wallet.Handlers;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Application.Services.Repositories;
using WalletApp.Domain.Entities;
using WalletApp.Domain.Enums;
using WalletApp.Infrastructure.Repositories;
using WalletApp.Infrastructure.Services.EmailServices;
using WalletApp.Persistence;
using WalletApp.Persistence.Base;
using WalletApp.Persistence.Repositories;
using WalletApp.WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Swagger grup desteği
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
            Array.Empty<string>()
        }
    });
});

// 🔹 DbContext
builder.Services.AddDbContext<WalletDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔹 JWT Authentication
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
        ValidateLifetime = false, // geliştirme ortamı için, prod'da true olmalı
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        RoleClaimType = System.Security.Claims.ClaimTypes.Role
    };
});

builder.Services.AddAuthorization();

// 🔹 E-posta ve diğer servisler
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, SmtpEmailService>();
builder.Services.AddScoped<WalletService>();

// 🔹 Repositories
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
builder.Services.AddScoped(typeof(IEntityRepository<>), typeof(EfEntityRepositoryBase<>));

// 🔹 MediatR + FluentValidation
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommandHandler).Assembly));

builder.Services.AddValidatorsFromAssemblyContaining<CreateBankAccountRequestValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// 🔹 Ortak servisler
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

var app = builder.Build();

// 🔹 Tek seferlik Admin seed
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
            PasswordHash = passwordHasher.HashPassword(null, password), // ilk parametre user nesnesi olabilir null da olur
            CreatedDate = DateTime.UtcNow
        };

        dbContext.Users.Add(admin);
        await dbContext.SaveChangesAsync();
    }
}

// 🔹 Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/Admin/swagger.json", "Admin API");
        options.SwaggerEndpoint("/swagger/Public/swagger.json", "Public API");
    });
}

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseMiddleware<AppUserMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
