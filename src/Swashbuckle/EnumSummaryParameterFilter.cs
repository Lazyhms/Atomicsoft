using System.ComponentModel;
using System.Reflection;
using System.Text;
using Microsoft.OpenApi.Models;

namespace Swashbuckle.AspNetCore.SwaggerGen;

internal sealed class EnumSummaryParameterFilter : IParameterFilter, IParameterAsyncFilter
{
    public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        => ReGenerateDescription(parameter, context);

    public async Task ApplyAsync(OpenApiParameter parameter, ParameterFilterContext context, CancellationToken cancellationToken)
    {
        ReGenerateDescription(parameter, context);

        await Task.CompletedTask;
    }

    private static void ReGenerateDescription(OpenApiParameter parameter, ParameterFilterContext context)
    {
        if (context.ParameterInfo.ParameterType.IsEnum)
        {
            var description = new StringBuilder(parameter.Description);
            var enumValues = Enum.GetValues(context.ParameterInfo.ParameterType);
            foreach (var value in enumValues)
            {
                var name = Enum.GetName(context.ParameterInfo.ParameterType, value);
                var field = context.ParameterInfo.ParameterType.GetField(name!);
                var attribute = field!.GetCustomAttribute<DescriptionAttribute>();
                description.Append($"<br/> {(int)value} : {attribute?.Description ?? name}");
            }
            parameter.Description = description.ToString();
        }
    }

}
