using System.Reflection;
using Swashbuckle.Application;

namespace Alka.SwaggerHelper
{
    /// <summary>
    /// integrate with the for fluid swagger configuration 
    /// </summary>
    public static class SwaggerDefaultValueValidatorExt
    {
        /// <summary>
        /// throw exception upon validation errors
        /// </summary>
        /// <param name="assembly"></param>
        public static void ValidateDefaultValues(this SwaggerDocsConfig config, Assembly assembly, bool allowNonExistingVersionParameter = false)
        {
            new SwaggerDefaultValueValidator().Validate(assembly, allowNonExistingVersionParameter);
        }
    }
}