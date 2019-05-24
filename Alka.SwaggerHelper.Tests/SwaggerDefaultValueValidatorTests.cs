using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Alka.SwaggerHelper.Tests
{
    [TestFixture]
    public class SwaggerDefaultValueValidatorTests
    {
        ValidationResult[] GetValidationErrors(string endpointName, bool cfg) => new SwaggerDefaultValueValidator().GetValidationErrors(GetMethods(endpointName), cfg);

        MethodInfo[] GetMethods(string name) => typeof(TestController).GetMethods().Where(x => x.Name == name).ToArray();

        [Test]
        public void When_all_ok_Then_no_errors()
        {
            var errors = GetValidationErrors("AllGood", false);

            Assert.IsEmpty(errors);
        }

        [Test]
        public void When_case_mismatch_Then_report_errors()
        {
            Assert.AreEqual(1, GetValidationErrors("AllGoodCaseMisMatch", false).Length);
        }

        [Test]
        public void When_version_is_defined_Then_fail_depending_on_configuration()
        {
            Assert.AreEqual(0, GetValidationErrors("AllGoodWithVersion", true).Length);
            Assert.AreEqual(1, GetValidationErrors("AllGoodWithVersion", false).Length);
        }

        [Test]
        public void When_duplicate_attribute_Then_report_error()
        {
            var errors = GetValidationErrors("DuplicateDefaultValue", false);

            Assert.AreEqual(1, errors.Length);
            var error = errors.First();
            Assert.AreEqual("Duplicate parameter", error.Description);
            Assert.AreEqual("Alka.SwaggerHelper.Tests.SwaggerDefaultValueValidatorTests+TestController.DuplicateDefaultValue", error.Method);
            Assert.AreEqual("s", error.Parameter);
        }

        [Test]
        public void When_unknown_attribute_Then_report_error()
        {
            var error = GetValidationErrors("ParameterNotFound", false).Single();

            Assert.AreEqual("Parameter not found", error.Description);
            Assert.AreEqual("Alka.SwaggerHelper.Tests.SwaggerDefaultValueValidatorTests+TestController.ParameterNotFound", error.Method);
            Assert.AreEqual("q", error.Parameter);
        }


        class TestController
        {
            [SwaggerDefaultValue("s", "")]
            public void AllGood(string s) { }

            [SwaggerDefaultValue("stringparam1", "")]
            public void AllGoodCaseMisMatch(string STRINGPARAM1) { }

            [SwaggerDefaultValue("s", "")]
            [SwaggerDefaultValue("version", "")]
            public void AllGoodWithVersion(string s) { }

            [SwaggerDefaultValue("s", "")]
            [SwaggerDefaultValue("s", "")]
            public void DuplicateDefaultValue(string s) { }

            [SwaggerDefaultValue("q", "")]
            public void ParameterNotFound(string s) { }
        }
    }
}
