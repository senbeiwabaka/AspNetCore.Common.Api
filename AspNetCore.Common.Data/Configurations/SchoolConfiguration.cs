using AspNetCore.Common.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNetCore.Common.Data.Configurations
{
    internal sealed class SchoolConfiguration : BaseConfiguration<School>
    {
        /// <inheritdoc/>
        public override void Configure(EntityTypeBuilder<School> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);

            base.Configure(builder);
        }
    }
}
