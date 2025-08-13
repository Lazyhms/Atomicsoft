using System.ComponentModel;
using System.Reflection;
using System.Text;
using Microsoft.OpenApi.Models;

namespace Swashbuckle.AspNetCore.SwaggerGen;

internal sealed class EnumSummarySchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            var description = new StringBuilder(schema.Description);
            var enumValues = Enum.GetValues(context.Type);
            foreach (var value in enumValues)
            {
                var name = Enum.GetName(context.Type, value);
                var field = context.Type.GetField(name!);
                var attribute = field!.GetCustomAttribute<DescriptionAttribute>();
                description.Append($"<br/> {(int)value} : {attribute?.Description ?? name}");
            }

            schema.Description = description.ToString();
        }
    }
}
