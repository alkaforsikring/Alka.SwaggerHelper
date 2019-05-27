# Alka.SwaggerHelper
swagger helper is a set of helper tools for working with swagger in .Net Framework Web api projects

To install this package use 

    PM> install-package alka.swaggerhelper


## SwaggerDefaultValues

This functionality enables you to 
* document arguments
* provide default values to the swagger ui


### 1. Getting started

To you swagger configuration add 

    public static void Register(HttpConfiguration config) 
    {
        config
            .EnableSwagger(..., c =>
            {
                c.OperationFilter<AddDefaultValues>();
                c.ValidateDefaultValues(Assembly.GetExecutingAssembly(), allowNonExistingVersionParameter: true);

This will both add your documentation and default values to the generated swagger ui, as well as validate your attributes against non-existing parameter names.


### 2. Decorate your controller methods 

        /// <summary>Subscribe a member</summary>
        /// <returns>A PartId identifier</returns>
        [ApiVersion("1.0")]
        [Route("SubscribeMember")]
        [HttpPost]
        [SwaggerDefaultValue("memberNumber", "{Nr=111111}", "A member number with or without separator character")]
        [SwaggerDefaultValue("version", "1", "version")]
        public async Task<HttpResponseMessage> SubscribeMember(MembershipRequest memberNumber)

        
### 3. Unit testing

It is adviceable to setup a validation step in a unit test so you get validation errors before releasing. Simply call the validator from a unittest

    [TestFixture]
    public class SwaggerDefaultValueValidatorTests
    {
        [Test]
        public void When_validating_defaultvalues_Then_no_errors_should_be_raised()
        {
            new SwaggerDefaultValueValidator()
                .Validate(typeof(MYCONTROLLER).Assembly, true)
        }
    }
 