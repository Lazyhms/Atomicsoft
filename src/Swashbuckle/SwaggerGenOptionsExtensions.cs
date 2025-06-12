using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Swashbuckle.AspNetCore.SwaggerGen;

public static class SwaggerGenOptionsExtensions
{
    private static readonly SecurityRequirementsOperationFilter JwtSecurityRequirementsOperationFilter = new("Bearer", new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme },
    });

    private static readonly SecurityRequirementsOperationFilter ApiKeySecurityRequirementsOperationFilter = new("ApiKey", new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference { Id = "ApiKey", Type = ReferenceType.SecurityScheme }
    });

    private static readonly SecurityRequirementsOperationFilter OAuthSecurityRequirementsOperationFilter = new("OAuth2", new OpenApiSecurityScheme
    {
        Name = "Bearer",
        Scheme = "oauth2",
        In = ParameterLocation.Header,
        Reference = new OpenApiReference { Id = "OAuth2", Type = ReferenceType.SecurityScheme, },
    });

    public static void IncludeXmlComments(this SwaggerGenOptions swaggerGenOptions)
        => swaggerGenOptions.IncludeXmlComments(AppContext.BaseDirectory, "*.xml");

    public static void IncludeXmlComments(this SwaggerGenOptions swaggerGenOptions, string path, string searchPattern)
        => Array.ForEach(Directory.GetFiles(path, searchPattern), xml => swaggerGenOptions.IncludeXmlComments(xml, true));

    public static void AddEnumSummarySchemaFilter(this SwaggerGenOptions swaggerGenOptions)
        => swaggerGenOptions.SchemaFilter<EnumSummarySchemaFilter>();

    public static void AddEnumSummaryParameterFilter(this SwaggerGenOptions swaggerGenOptions)
        => swaggerGenOptions.AddParameterFilterInstance(new EnumSummaryParameterFilter());

    public static void AddEnumSummaryParameterAsyncFilter(this SwaggerGenOptions swaggerGenOptions)
        => swaggerGenOptions.AddParameterAsyncFilterInstance(new EnumSummaryParameterFilter());

    public static void AddJwtBearerSecurityDefinitionAndFilter(this SwaggerGenOptions swaggerGenOptions)
    {
        swaggerGenOptions.AddJwtBearerSecurityDefinition();
        swaggerGenOptions.AddOperationFilterInstance(JwtSecurityRequirementsOperationFilter);
    }

    public static void AddJwtBearerSecurityDefinitionAndAsyncFilter(this SwaggerGenOptions swaggerGenOptions)
    {
        swaggerGenOptions.AddJwtBearerSecurityDefinition();
        swaggerGenOptions.AddOperationAsyncFilterInstance(JwtSecurityRequirementsOperationFilter);
    }

    public static void AddApiKeySecurityDefinitionAndFilter(this SwaggerGenOptions swaggerGenOptions)
    {
        swaggerGenOptions.AddApiKeySecurityDefinition();
        swaggerGenOptions.AddOperationFilterInstance(ApiKeySecurityRequirementsOperationFilter);
    }

    public static void AddApiKeySecurityDefinitionAndAsyncFilter(this SwaggerGenOptions swaggerGenOptions)
    {
        swaggerGenOptions.AddApiKeySecurityDefinition();
        swaggerGenOptions.AddOperationAsyncFilterInstance(ApiKeySecurityRequirementsOperationFilter);
    }

    public static void AddOAuthSecurityDefinitionAndFilter(this SwaggerGenOptions swaggerGenOptions, OpenApiOAuthFlow openApiOAuthFlow)
    {
        swaggerGenOptions.AddOAuthSecurityDefinition(openApiOAuthFlow);
        swaggerGenOptions.AddOperationFilterInstance(OAuthSecurityRequirementsOperationFilter);
    }

    public static void AddOAuthSecurityDefinitionAndAsyncFilter(this SwaggerGenOptions swaggerGenOptions, OpenApiOAuthFlow openApiOAuthFlow)
    {
        swaggerGenOptions.AddOAuthSecurityDefinition(openApiOAuthFlow);
        swaggerGenOptions.AddOperationAsyncFilterInstance(OAuthSecurityRequirementsOperationFilter);
    }

    private static void AddOAuthSecurityDefinition(this SwaggerGenOptions swaggerGenOptions, OpenApiOAuthFlow openApiOAuthFlow) => swaggerGenOptions.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows { ClientCredentials = openApiOAuthFlow }
    });

    private static void AddApiKeySecurityDefinition(this SwaggerGenOptions swaggerGenOptions) => swaggerGenOptions.AddSecurityDefinition("ApiKey", new()
    {
        Name = "X-API-KEY",
        Type = SecuritySchemeType.ApiKey,
        Description = "X-API-KEY Authorization",
        In = ParameterLocation.Header | ParameterLocation.Query,
    });

    private static void AddJwtBearerSecurityDefinition(this SwaggerGenOptions swaggerGenOptions) => swaggerGenOptions.AddSecurityDefinition("Bearer", new()
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "JWT Authorization header: Bearer {token}",
    });
}
