using AspNetCore.Common.Data;
using AspNetCore.Common.Shared;
using AspNetCore.Common.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace AspNetCore.Common.Domain
{
    public abstract class ReadonlyRepository<TEntity> : IReadonlyRepository<TEntity>
        where TEntity : BaseModel
    {
        private readonly Type parentEntityType = typeof(TEntity);
        private readonly PropertyInfo[] parentProperties;

        protected ReadonlyRepository(AppDbContext context)
        {
            Context = context;

            parentProperties = parentEntityType.GetProperties();
        }

        protected AppDbContext Context { get; private set; }

        public virtual Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Context.Set<TEntity>().SingleOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
        }

        public virtual async Task<PageResult<TEntity>> GetListAsync(
            int page,
            int pageSize,
            IReadOnlyList<Filter>? filters,
            IReadOnlyDictionary<string, string>? sorting,
            CancellationToken cancellationToken)
        {
            var query = Context.Set<TEntity>().AsQueryable();

            if (filters is not null)
            {
                for (var i = 0; i < filters.Count; i++)
                {
                    var item = filters[i];

                    query = query.Where($"{item.Property}{GetEvaluationType(item.Property, item.FilterType, i)}", item.Value);
                }
            }

            var count = await query.CountAsync(cancellationToken).ConfigureAwait(false);

            if (sorting is not null && sorting.Any())
            {
                var ordering = new StringBuilder();

                for (var i = 0; i < sorting.Count; i++)
                {
                    var element = sorting.ElementAt(i);

                    if (i == 0)
                    {
                        ordering.Append($"{element.Key} {element.Value}");
                    }
                    else
                    {
                        ordering.Append($", {element.Key} {element.Value}");
                    }
                }

                query = query.OrderBy(ordering.ToString());
            }

            query = query.Skip(page * pageSize).Take(pageSize);

            var data = await query.ToListAsync(cancellationToken).ConfigureAwait(false);

            return new PageResult<TEntity>
            {
                Items = data,
                TotalCount = count,
            };
        }

        protected virtual string GetEvaluationType(string property, FilterType filterType, int index)
        {
            // Null throw should never happen
            var parentProperty = parentProperties.Single(x => string.Equals(x.Name, property, StringComparison.InvariantCultureIgnoreCase));

            if (parentProperty.PropertyType.IsClass &&
                parentProperty.PropertyType.Assembly.FullName == parentEntityType.Assembly.FullName)
            {
                // Null throw should never happen
                var childProperty = parentProperty.PropertyType.GetProperty(property) ?? throw new Exception("Property didn't exist");
                var childType = childProperty.PropertyType;

                if (childType == typeof(string))
                {
                    return GetStringFilterType(filterType, index);
                }

                if (childType == typeof(int) ||
                    childType == typeof(double) ||
                    childType == typeof(long) ||
                    childType == typeof(float))
                {
                    return GetNumericFilterType(filterType, index);
                }
            }

            var parentPropertyType = parentProperty.PropertyType;

            if (parentPropertyType == typeof(string))
            {
                return GetStringFilterType(filterType, index);
            }

            if (parentPropertyType == typeof(int) ||
                parentPropertyType == typeof(double) ||
                parentPropertyType == typeof(long) ||
                parentPropertyType == typeof(float))
            {
                return GetNumericFilterType(filterType, index);
            }

            // This should never happen
            throw new Exception("No filter viable");
        }

        protected virtual string GetSortOrder(SortType sortType)
        {
            return sortType == SortType.Ascending ? "asc" : "desc";
        }

        private static string GetStringFilterType(FilterType filterType, int index)
        {
            if (filterType == FilterType.Equal)
            {
                return $" == @{index}";
            }

            if (filterType == FilterType.NotEqual)
            {
                return $" != @{index}";
            }

            if (filterType == FilterType.Contains)
            {
                return $".Contains(@{index})";
            }

            // This should never happen
            throw new Exception("Invalid filter type selected for string");
        }

        private static string GetNumericFilterType(FilterType filterType, int index)
        {
            if (filterType == FilterType.Equal)
            {
                return $" == @{index}";
            }

            if (filterType == FilterType.NotEqual)
            {
                return $" != @{index}";
            }

            if (filterType == FilterType.LessThan)
            {
                return $" < @{index}";
            }

            if (filterType == FilterType.GreaterThan)
            {
                return $" > @{index}";
            }

            if (filterType == FilterType.LessThanOrEqual)
            {
                return $" <= @{index}";
            }

            if (filterType == FilterType.GreaterThanOrEqual)
            {
                return $" >= @{index}";
            }

            // This should never happen
            throw new Exception("Invalid filter type selected for numeric");
        }
    }
}
