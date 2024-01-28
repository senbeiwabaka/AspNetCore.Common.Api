using AspNetCore.Common.Models;
using AspNetCore.Common.Shared.Models;
using FluentValidation;

namespace AspNetCore.Common.Api.Validations.V1
{
    internal sealed class SchoolRequestModelValidator : RequestModelValidator<School>
    {
        public SchoolRequestModelValidator()
        {
        }
    }
}
