using AspNetCore.Common.Models;
using FluentValidation;

namespace AspNetCore.Common.Api.Validations.V1
{
    internal sealed class SchoolValidator : AbstractValidator<School>
    {
        public SchoolValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(100);
        }
    }
}
