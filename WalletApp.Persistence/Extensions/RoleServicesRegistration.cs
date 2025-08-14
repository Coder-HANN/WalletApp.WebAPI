using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;


namespace WalletApp.Persistence.Extensions
{
    public static class RoleServicesRegistration
    {
        public static IServiceCollection AddRoleServices(this IServiceCollection services)
        {
            services.AddSwaggerGen( options =>
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

            return services;
        }
    }
}
