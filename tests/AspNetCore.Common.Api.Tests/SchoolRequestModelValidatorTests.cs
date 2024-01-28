using AspNetCore.Common.Api.Validations.V1;
using FluentValidation.TestHelper;

namespace AspNetCore.Common.Api.Tests
{
    internal sealed class SchoolRequestModelValidatorTests
    {
        private SchoolRequestModelValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new SchoolRequestModelValidator();
        }

        [Test]
        public void PropertyNameDoesNotExist()
        {
            // Arrange
            var model = new RequestModel
            {
                Filters = new List<Shared.Models.Filter>
                {
                    new Shared.Models.Filter
                    {
                        Property = "cool",
                    }
                }
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Filters);
        }

        [Test]
        public void PropertyNameCaseDoesNotMatter()
        {
            // Arrange
            var model = new RequestModel
            {
                Filters = new List<Shared.Models.Filter>
                {
                    new Shared.Models.Filter
                    {
                        Property = "name",
                    }
                }
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Filters);
        }
    }
}
