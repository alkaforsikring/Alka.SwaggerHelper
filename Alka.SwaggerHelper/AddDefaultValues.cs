using System.Linq;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace Alka.SwaggerHelper
{
    /// <summary>
    /// Swashbuckle integration that reads the <see cref="SwaggerDefaultValue"/> annotations
    /// </summary>
    public class AddDefaultValues : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
                return;

            foreach (var param in operation.parameters)
            {
                var customValue = apiDescription.ActionDescriptor
                    .GetCustomAttributes<SwaggerDefaultValue>()
                    .FirstOrDefault(p => p.ParameterName == param.name);

                if (customValue != null)
                {
                    param.@default = customValue.Value;
                    param.description = customValue.Description;
                }
            }
        }
    }
}