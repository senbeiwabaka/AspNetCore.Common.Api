using AspNetCore.Common.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNetCore.Common.Data.Configurations
{
    internal sealed class AddressConfiguration : BaseConfiguration<Address>
    {
        /// <inheritdoc/>
        public override void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(x => x.Line1).IsRequired().HasMaxLength(500);
            builder.Property(x => x.Line2).HasMaxLength(500);

            base.Configure(builder);
        }
    }
}
