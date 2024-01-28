
using AspNetCore.Common.Shared.Models;

namespace AspNetCore.Common.Shared
{
    public interface IReadonlyRepository<TEntity>
        where TEntity : BaseModel
    {
        Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<PageResult<TEntity>> GetListAsync(
            int page,
            int pageSize,
            IReadOnlyList<Filter>? filters,
            IReadOnlyDictionary<string, string>? sorting,
            CancellationToken cancellationToken);
    }
}
