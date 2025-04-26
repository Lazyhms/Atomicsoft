using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Mvc.Filters;

public sealed class GlobalExceptionFilterAttribute(
    IOptions<ExceptionResult> options,
    IWebHostEnvironment webHostEnvironment,
    ILogger<GlobalExceptionFilterAttribute> logger) : ExceptionFilterAttribute
{
    private readonly ExceptionResult _globalExceptionResult = options.Value;

    public override Task OnExceptionAsync(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case BizException bizException:
                logger.LogError(bizException, "Title:BizException HResult:{HResult}", bizException.HResult);

                context.ExceptionHandled = true;
                _globalExceptionResult.Message = bizException.Message;
                context.Result = new ObjectResult(_globalExceptionResult);
                break;
            case Exception handledException:
                logger.LogError(handledException, "Title:Exception HResult:{HResult}", handledException.HResult);

                context.ExceptionHandled = true;
                _globalExceptionResult.Message = webHostEnvironment.IsDevelopment()
                    ? handledException.ToString() : "Internal server error, Please contact the administrator";
                context.Result = new ObjectResult(_globalExceptionResult);
                break;
        }

        return base.OnExceptionAsync(context);
    }
}
