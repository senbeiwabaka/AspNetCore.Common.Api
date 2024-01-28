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
                        var property = entityProperties.SingleOrDefault(x => string.Equals(x.Name, filter.Property, StringComparison.InvariantCultureIgnoreCase));

                        if (property is null)
                        {
                            return false;
                        }

                        if (!Enum.IsDefined(typeof(FilterType), filter.FilterType))
                        {
                            return false;
                        }

                        if (property.PropertyType == typeof(string) && !GetStringValidFilterType(filter.FilterType))
                        {
                            return false;
                        }

                        if (property.PropertyType == typeof(bool) && !GetBooleanValidFilterType(filter.FilterType))
                        {
                            return false;
                        }

                        if ((property.PropertyType == typeof(int) ||
                        property.PropertyType == typeof(double) ||
                        property.PropertyType == typeof(long) ||
                        property.PropertyType == typeof(float)) &&
                        !GetNumericValidFilterType(filter.FilterType))
                        {
                            return false;
                        }
                    }

                    return true;
                });
        }

        private static bool GetNumericValidFilterType(FilterType selectedFilterType)
        {
            return !(selectedFilterType == FilterType.Contains ||
                selectedFilterType == FilterType.DoesNotContain);
        }

        private static bool GetStringValidFilterType(FilterType selectedFilterType)
        {
            return selectedFilterType == FilterType.Equal ||
                selectedFilterType == FilterType.NotEqual ||
                selectedFilterType == FilterType.Contains ||
                selectedFilterType == FilterType.DoesNotContain;
        }

        private static bool GetBooleanValidFilterType(FilterType selectedFilterType)
        {
            return selectedFilterType == FilterType.Equal ||
                selectedFilterType == FilterType.NotEqual;
        }
    }
}
