using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WalletApp.Application.Abstraction.Repositories;
using WalletApp.Application.Abstraction.Repositories.EntitysRepository;
using WalletApp.Application.DTO;
using WalletApp.Application.Handler.RegisterUserCommandHandler;
using WalletApp.Domain.Base;
using WalletApp.Infrastructure.Repositories;
using WalletApp.Persistence;
using WalletApp.Persistence.Base;
using WalletApp.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------
// Dependency Injection (DI)
// -----------------------------
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// Scoped olarak repository'leri ekle
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Generic repository
builder.Services.AddScoped(typeof(IEntityRepository<>), typeof(EfEntityRepositoryBase<>));

// DbContext
builder.Services.AddDbContext<WalletDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(
        typeof(RegisterUserCommandHandler).Assembly,
        typeof(RegisterUserCommand).Assembly
    ));

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
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Wallet API",
        Version = "v1",
        Description = "Sigorta Stajı İçin Wallet API Projesi"
    });

    // ➕ Swagger'a JWT için Authorization header ekle
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT token'ı buraya girin: Bearer {token}"
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

app.UseHttpsRedirection();

app.UseAuthentication(); // ➕ JWT doğrulaması
app.UseAuthorization();

app.MapControllers();

app.Run();
