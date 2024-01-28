using AspNetCore.Common.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNetCore.Common.Data.Configurations
{
    internal abstract class BaseConfiguration<T> : IEntityTypeConfiguration<T>
        where T : BaseModel
    {
        /// <inheritdoc/>
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            if (builder is null)
            {
                throw new System.ArgumentNullException(nameof(builder));
            }

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Created).IsRequired();
            builder.Property(x => x.Updated).IsRequired();
        }
    }
}
