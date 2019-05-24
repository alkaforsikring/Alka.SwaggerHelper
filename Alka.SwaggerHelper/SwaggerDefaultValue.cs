using System;

namespace Alka.SwaggerHelper
{
    /// <summary>
    /// Attribute to enrich endpoint methods with swagger information
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SwaggerDefaultValue : Attribute
    {
        public string ParameterName { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

        public SwaggerDefaultValue(string parameterName, string value, string description = null)
        {
            ParameterName = parameterName;
            Value = value;
            Description = description;
        }
    }
}