using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Persistence.Extensions
{
    public static class SwaggerRegistration
    {
        public static IServiceCollection AddSwaggerGenRegistration(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
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
                { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, Array.Empty<string>() }
            });
            });
            return services;
        }
    }
}
