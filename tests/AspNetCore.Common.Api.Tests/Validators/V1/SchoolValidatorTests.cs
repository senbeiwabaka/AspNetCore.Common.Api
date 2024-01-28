using AspNetCore.Common.Api.Validations.V1;
using AspNetCore.Common.Models;
using FluentValidation.TestHelper;

namespace AspNetCore.Common.Api.Tests.Validators.V1
{
    internal sealed class SchoolValidatorTests
    {
        private SchoolValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new SchoolValidator();
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase(" ")]
        [TestCase("12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890")]
        public void NameValidation(string? name)
        {
            // Arrange
            var model = new School
            {
                Name = name,
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
    }
}
