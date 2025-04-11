using Microsoft.AspNetCore.Diagnostics;

namespace Microsoft.Extensions.DependencyInjection;

public static class ExceptionHandlerServiceExtensions
{
    public static IServiceCollection AddExceptionHandler(this IServiceCollection services)
        => services.ConfigureGlobalResult().AddExceptionHandler<GlobalExceptionHandler>();
}
