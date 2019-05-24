using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alka.SwaggerHelper
{
    public class SwaggerDefaultValueValidator
    {
        /// <summary>
        /// throw exception upon validation errors
        /// </summary>
        public void Validate(Assembly assembly, bool allowNonExistingVersionParameter)
        {
            var validationErrors = GetValidationErrors(assembly, allowNonExistingVersionParameter);

            if (validationErrors.Any())
            {
                var message = $"{validationErrors.Length} wrong SwaggerDefaultValues: " + string.Join("\n,", validationErrors.Select(x => x.ToString()));
                throw new System.Exception(message);
            }
        }

        /// <summary>
        /// Returns the set of validation errors
        /// </summary>
        public ValidationResult[] GetValidationErrors(Assembly assembly, bool allowNonExistingVersionParameter)
        {
            return GetValidationErrors(assembly.DefinedTypes.SelectMany(x => x.DeclaredMethods), allowNonExistingVersionParameter);
        }

        /// <summary>
        /// Use this method to validate a set of methods, e.g. use it in a unit test to ensure that annotations are correct
        /// </summary>
        public ValidationResult[] GetValidationErrors(
            IEnumerable<MethodInfo> methodInfos,
            bool allowNonExistingVersionParameter)
        {
            var endpoints = methodInfos
                .Select(x => new
                {
                    MethodName = $"{x.DeclaringType}.{x.Name}",
                    Parameters = x.GetParameters().Select(p => p.Name).ToArray(),
                    Attributes = x.GetCustomAttributes<SwaggerDefaultValue>()
                })
                .ToArray();

            var nonMatches = endpoints
                .SelectMany(x =>
                        x.Attributes.Where(attr => x.Parameters.All(p => p != attr.ParameterName)),
                    (m, nonMatch) => new ValidationResult("Parameter not found", m.MethodName, nonMatch.ParameterName))
                .Where(x => allowNonExistingVersionParameter ? x.Parameter != "version" : true);

            var duplicates = endpoints
                .SelectMany(x =>
                        x.Attributes.Select(a => a.ParameterName)
                            .Distinct()
                            .Where(attributeName => x.Attributes.Count(c => c.ParameterName == attributeName) > 1),
                    (m, duplicate) => new ValidationResult("Duplicate parameter", m.MethodName, duplicate));

            return nonMatches.Concat(duplicates).ToArray();
        }
    }
}