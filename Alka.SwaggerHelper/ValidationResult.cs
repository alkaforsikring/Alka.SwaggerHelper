namespace Alka.SwaggerHelper
{
    public class ValidationResult
    {
        public readonly string Description, Method, Parameter;

        public ValidationResult(string description, string method, string parameter)
        {
            Description = description;
            Method = method;
            Parameter = parameter;
        }

        public override string ToString() => $"{Description}: {Method}(parameter:{Parameter})";
    }
}
