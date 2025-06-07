using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Swashbuckle.AspNetCore.SwaggerGen;

public static class SwaggerGenOptionsExtensions
{
    public static void AddJwtBearerSecurityDefinition(this SwaggerGenOptions swaggerGenOptions)
    {
        swaggerGenOptions.AddSecurityDefinition("Bearer", new()
        {
            Scheme = "Bearer",
            BearerFormat = "JWT",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Description = "JWT Authorization header: Bearer {token}",
        });
        swaggerGenOptions.AddOperationAsyncFilterInstance(new SecurityRequirementsOperationFilter("Bearer", new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme },
        }));
    }

    public static void AddApiKeySecurityDefinition(this SwaggerGenOptions swaggerGenOptions)
    {
        swaggerGenOptions.AddSecurityDefinition("ApiKey", new()
        {
            Name = "X-API-KEY",
            Type = SecuritySchemeType.ApiKey,
            Description = "X-API-KEY Authorization",
            In = ParameterLocation.Header | ParameterLocation.Query,
        });
        swaggerGenOptions.AddOperationAsyncFilterInstance(new SecurityRequirementsOperationFilter("ApiKey", new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference { Id = "ApiKey", Type = ReferenceType.SecurityScheme }
        }));
    }

    public static void AddOAuthSecurityDefinition(this SwaggerGenOptions swaggerGenOptions, OpenApiOAuthFlow authFlow)
    {
        swaggerGenOptions.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                ClientCredentials = authFlow
            }
        });

        swaggerGenOptions.AddOperationAsyncFilterInstance(new SecurityRequirementsOperationFilter("Bearer", new OpenApiSecurityScheme
        {
            Name = "Bearer",
            Scheme = "oauth2",
            In = ParameterLocation.Header,
            Reference = new OpenApiReference { Id = "OAuth2", Type = ReferenceType.SecurityScheme, },
        }));
    }

    public static void AddReadOnlySchemaFilter(this SwaggerGenOptions swaggerGenOptions)
        => swaggerGenOptions.SchemaFilter<ReadOnlySchemaFilter>();
}
