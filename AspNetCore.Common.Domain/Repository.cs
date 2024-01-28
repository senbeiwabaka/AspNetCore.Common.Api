using AspNetCore.Common.Data;
using AspNetCore.Common.Shared;
using AspNetCore.Common.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Common.Domain
{
    public abstract class Repository<TEntity> : IRepository<TEntity>
        where TEntity : BaseModel
    {
        protected Repository(AppDbContext context)
        {
            Context = context;
        }

        protected AppDbContext Context { get; private set; }

        public virtual async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            _ = Context.Add(entity);

            _ = await Context.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return entity;
        }

        public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

            Context.Set<TEntity>().Remove(entity);

            _ = await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Context.Set<TEntity>().SingleOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
        }

        public virtual Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
