using Microsoft.OpenApi.Models;

namespace Swashbuckle.AspNetCore.SwaggerGen;

public sealed class ReadOnlySchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties == null)
        {
            return;
        }

        foreach (var property in context.Type.GetProperties())
        {
            var attributes = property.GetCustomAttributes(inherit: true);
            if (schema.Properties.Keys.FirstOrDefault(o => o.Equals(property.Name)) is string key && !string.IsNullOrWhiteSpace(key))
            {
                if (schema.Properties[key].ReadOnly)
                {
                    continue;
                }

                if (property.SetMethod == null || property.SetMethod.IsPrivate)
                {
                    schema.Properties[key].ReadOnly = true;
                }
            }
        }
    }
}
