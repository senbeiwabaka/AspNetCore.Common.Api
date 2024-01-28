using AspNetCore.Common.Shared.Models;

namespace AspNetCore.Common.Models
{
    public sealed class Address : BaseModel
    {
        public string Line1 { get; set; } = default!;

        public string? Line2 { get; set; }

        public string State { get; set; } = default!;

        public string City { get; set; } = default!;

        public string Zip { get; set; } = default!;
    }
}
