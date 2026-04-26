using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NortwestV2.Api.SchemaFilters;

public class RemoveNullableSchemaFilter : ISchemaFilter
{
    public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties == null)
            return;

        foreach (var property in schema.Properties.Values)
        {
            if (property is not OpenApiSchema concreteProperty)
            {
                continue;
            }

            if (concreteProperty.AnyOf?.Count > 0)
            {
                var nonNullTypes = concreteProperty.AnyOf
                    .Where(s => s.Type == JsonSchemaType.Null)
                    .ToList();

                concreteProperty.AnyOf.Clear();
                foreach (var t in nonNullTypes)
                    concreteProperty.AnyOf.Add(t);
            }
        }
    }
}