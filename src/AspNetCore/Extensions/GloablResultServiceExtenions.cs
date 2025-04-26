using Microsoft.AspNetCore.Mvc;

namespace Microsoft.Extensions.DependencyInjection;

public static class GloablResultServiceExtenions
{
    /// <summary>
    /// Configure the global return <see cref="ExceptionResult"/> format
    /// </summary>
    public static IServiceCollection ConfigureGlobalResult(this IServiceCollection services)
        => services
            .ConfigureSuccessResult(options => { options.Code = 1; options.Message = "Success."; })
            .ConfigureGlobalException(options => { options.Code = 0; options.Message = "InternalServerError."; });

    /// <summary>
    /// This should be used when no exception is thrown.
    /// </summary>
    public static IServiceCollection ConfigureSuccessResult(this IServiceCollection services, Action<GlobalObjectResult> configureOptions)
        => services.Configure<GlobalObjectResult>(options => configureOptions(options));

    /// <summary>
    /// This should be used when an <see cref="Exception"/> is thrown.
    /// </summary>
    public static IServiceCollection ConfigureGlobalException(this IServiceCollection services, Action<ExceptionResult> configureOptions)
        => services.Configure<ExceptionResult>(options => configureOptions(options));
}
