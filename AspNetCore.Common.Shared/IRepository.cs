using AspNetCore.Common.Shared.Models;

namespace AspNetCore.Common.Shared
{
    public interface IRepository<TEntity>
        where TEntity : BaseModel
    {
        Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken);

        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken);

        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
