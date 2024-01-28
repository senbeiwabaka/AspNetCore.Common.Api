using AspNetCore.Common.Shared.Models;

namespace AspNetCore.Common.Models
{
    public sealed class School : BaseModel
    {
        public string Name { get; set; } = default!;

        public Address? Address { get; set; }
    }
}
