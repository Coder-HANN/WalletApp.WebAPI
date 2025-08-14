using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WalletApp.Application.Abstraction.Services;
using WalletApp.Domain.Entities;
using WalletApp.Domain.Enums;
using WalletApp.Infrastructure.Services.EmailServices;
using WalletApp.Persistence.Context;
using WalletApp.Persistence.Extensions;
using WalletApp.WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// --- SERILOG CONFIGURATION ---
builder.Host.AddLogService();

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
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// HttpContextAccessor ve CurrentUserService
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// JWT service
builder.Services.AddJwtService(builder.Configuration);
builder.Services.AddAuthorization();

// Controllers
builder.Services.AddControllers();
builder.Services.AddControllersWithViews()
       .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Program>());

// Swagger + Role-based API Docs
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRoleServices();

// Email ve diğer servisler
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, SmtpEmailService>();

// Repositories & Services
builder.Services.AddApplicationServices();

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommandHandler).Assembly));

// MemoryCache
builder.Services.AddMemoryCache();

var app = builder.Build();

// Admin user seed
await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<WalletDbContext>();
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<AppUser>>();

    var email = config["AdminUser:Email"];
    var password = config["AdminUser:Password"];

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

app.UseMiddleware<RequestLoggingMiddleware>();

app.MapControllers();

app.Run();
