using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

namespace Swashbuckle.AspNetCore.SwaggerGen;

internal sealed class SecurityRequirementsOperationFilter(string authenticationScheme, OpenApiSecurityScheme openApiSecurityScheme) : IOperationFilter, IOperationAsyncFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.CustomAttributes().OfType<AllowAnonymousAttribute>().Any())
        {
            return;
        }
        if (!context.ApiDescription.CustomAttributes().OfType<AuthorizeAttribute>().Any())
        {
            return;
        }
        if (!operation.Security.Any(requirement => requirement.Any(scheme => scheme.Key.Reference.Id == authenticationScheme)))
        {
            operation.Security.Add(new OpenApiSecurityRequirement { { openApiSecurityScheme, Array.Empty<string>() } });
        }
    }

    public async Task ApplyAsync(OpenApiOperation operation, OperationFilterContext context, CancellationToken cancellationToken)
    {
        if (context.ApiDescription.CustomAttributes().OfType<AllowAnonymousAttribute>().Any())
        {
            return;
        }
        if (!context.ApiDescription.CustomAttributes().OfType<AuthorizeAttribute>().Any())
        {
            return;
        }
        if (!operation.Security.Any(requirement => requirement.Any(scheme => scheme.Key.Reference.Id == authenticationScheme)))
        {
            operation.Security.Add(new OpenApiSecurityRequirement { { openApiSecurityScheme, Array.Empty<string>() } });
        }
        await Task.CompletedTask;
    }
}
