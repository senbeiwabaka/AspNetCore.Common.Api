using AspNetCore.Common.Shared.Models;
using FluentValidation;

namespace AspNetCore.Common.Api.Validations.V1
{
    public class RequestModelValidator<TModel> : AbstractValidator<RequestModel>
        where TModel : BaseModel
    {
        private readonly Type entityType = typeof(TModel);

        public RequestModelValidator()
        {
            RuleFor(x => x.Filters)
                .Must(filters =>
                {
                    if (filters is null)
                    {
                        return true;
                    }

                    var entityProperties = entityType.GetProperties();

                    foreach (var filter in filters)
                    {
                        if (!Array.Exists(entityProperties, x => string.Equals(x.Name, filter.Property, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            return false;
                        }
                    }

                    return true;
                });
        }
    }
}
