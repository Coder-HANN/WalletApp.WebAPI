using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WalletApp.Application.Feature.Auth.Handlers;
using WalletApp.Application.Feature.BankAccount.Handlers;
using WalletApp.Application.Feature.BankAccount.Validations;
using WalletApp.Application.Feature.Wallet.Handlers;
using WalletApp.Application.Feature.Wallet.Validations;
using WalletApp.Application.Services;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Application.Services.Repositories;
using WalletApp.Domain.Entities;
using WalletApp.Infrastructure.Repositories;
using WalletApp.Infrastructure.Services.EmailServices;
using WalletApp.Persistence;
using WalletApp.Persistence.Base;
using WalletApp.Persistence.Repositories;
using WalletApp.WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------
// Dependency Injection (DI)
// -----------------------------
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IEmailService, SmtpEmailService>();
builder.Services.AddScoped<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();
builder.Services.AddScoped<WalletService>();

// Scoped olarak repository'leri ekle
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Generic repository
builder.Services.AddScoped(typeof(IEntityRepository<>), typeof(EfEntityRepositoryBase<>));

// DbContext (sadece bir kere eklenmeli)
builder.Services.AddDbContext<WalletDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    ));


// MediatR: sadece bir kez, tüm handler assembly'leri ekleniyor
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(
        typeof(RegisterUserCommandHandler).Assembly,
        typeof(BankTransferCommandHandler).Assembly,
       typeof(BankDepositCommandHandler).Assembly
    ));

// Diğer repository'ler
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IUserDetailRepository, UserDetailRepository>();
builder.Services.AddScoped<IWalletTransferRepository, WalletTransferRepository>();
builder.Services.AddScoped<IBankTransactionRepository, BankTransactionRepository>();
builder.Services.AddScoped<IProviderBankRepository, ProviderBankRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();



// Fluent Validation
builder.Services.AddValidatorsFromAssemblyContaining<AppWalletCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateBankAccountRequestValidator>();

// Pipeline Behavior (Validation)
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// HTTP Context Accessor
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// -----------------------------
// JWT Authentication
// -----------------------------
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
        ValidateLifetime = false, // Süresiz token
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();

// -----------------------------
// Controllers & Swagger
// -----------------------------
builder.Services.AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Wallet API",
        Version = "v1",
        Description = "Staj İçin Wallet API Projesi"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Tokenınızı girin"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

// -----------------------------
// App Pipeline
// -----------------------------
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Wallet API v1");
    });
}

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseMiddleware<AppUserMiddleware>(); // 🧠 Burada devreye giriyor!
app.UseAuthorization();

app.MapControllers();

app.Run();
