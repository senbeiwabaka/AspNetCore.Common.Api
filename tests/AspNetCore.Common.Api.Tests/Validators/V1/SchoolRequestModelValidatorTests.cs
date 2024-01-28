using AspNetCore.Common.Api.Validations.V1;
using AspNetCore.Common.Shared.Models;
using FluentValidation.TestHelper;

namespace AspNetCore.Common.Api.Tests.Validators.V1
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
                Filters = new List<Filter>
                {
                    new Filter
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
                Filters = new List<Filter>
                {
                    new Filter
                    {
                        Property = "name",
                        FilterType = FilterType.Equal,
                    }
                }
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Filters);
        }

        [TestCase(FilterType.GreaterThan)]
        [TestCase(FilterType.LessThan)]
        [TestCase(FilterType.GreaterThanOrEqual)]
        [TestCase(FilterType.LessThanOrEqual)]
        public void PropertyWrongFilterTypeString(FilterType filterType)
        {
            // Arrange
            var model = new RequestModel
            {
                Filters = new List<Filter>
                {
                    new Filter
                    {
                        Property = "name",
                        FilterType = filterType,
                    }
                }
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Filters);
        }

        [TestCase(FilterType.Equal)]
        [TestCase(FilterType.NotEqual)]
        [TestCase(FilterType.Contains)]
        [TestCase(FilterType.DoesNotContain)]
        public void PropertyFilterTypeString(FilterType filterType)
        {
            // Arrange
            var model = new RequestModel
            {
                Filters = new List<Filter>
                {
                    new Filter
                    {
                        Property = "name",
                        FilterType = filterType,
                    }
                }
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Filters);
        }

        [TestCase(FilterType.GreaterThan)]
        [TestCase(FilterType.LessThan)]
        [TestCase(FilterType.GreaterThanOrEqual)]
        [TestCase(FilterType.LessThanOrEqual)]
        [TestCase(FilterType.Contains)]
        [TestCase(FilterType.DoesNotContain)]
        [TestCase(FilterType.StartsWith)]
        [TestCase(FilterType.EndsWith)]
        public void PropertyWrongFilterTypeBoolean(FilterType filterType)
        {
            // Arrange
            var model = new RequestModel
            {
                Filters = new List<Filter>
                {
                    new Filter
                    {
                        Property = "isused",
                        FilterType = filterType,
                    }
                }
            };

            // Act
            var result = validator.TestValidate(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Filters);
        }

        [TestCase(FilterType.Equal)]
        [TestCase(FilterType.NotEqual)]
        public void PropertyFilterTypeBoolean(FilterType filterType)
        {
            // Arrange
            var model = new RequestModel
            {
                Filters = new List<Filter>
                {
                    new Filter
                    {
                        Property = "isused",
                        FilterType = filterType,
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
