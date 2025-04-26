using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Diagnostics;

public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IOptions<ExceptionResult> exceptionResult,
    IWebHostEnvironment webHostEnvironment) : IExceptionHandler
{
    private readonly ExceptionResult _globalExceptionResult = exceptionResult.Value;

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        switch (exception)
        {
            case BizException bizException:
                logger.LogError(bizException, "Title:BizException HResult:{HResult}", bizException.HResult);

                _globalExceptionResult.Message = bizException.Message;
                await context.Response.WriteAsJsonAsync(_globalExceptionResult, cancellationToken);
                return await ValueTask.FromResult(true);
            case Exception handledException:
                logger.LogError(handledException, "Title:Exception HResult:{HResult}", handledException.HResult);

                _globalExceptionResult.Message = webHostEnvironment.IsDevelopment()
                    ? handledException.ToString() : "服务器发生错误,请联系管理员";
                await context.Response.WriteAsJsonAsync(_globalExceptionResult, cancellationToken);
                return await ValueTask.FromResult(true);
        }
        return await ValueTask.FromResult(false);
    }
}
