using AspNetCore.Common.Shared.Models;

namespace AspNetCore.Common.Models
{
    public sealed class School : BaseModel
    {
        public string Name { get; set; } = default!;

        public bool IsUsed { get; set; }

        public Guid? AddressId { get; set; }
    }
}
