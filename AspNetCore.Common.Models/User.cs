namespace AspNetCore.Common.Models
{
    public sealed class User
    {
        public Guid Id { get; set; }

        public string LegalFirstName { get; set; } = default!;

        public string LegalLastName { get; set; } = default!;

        public string? UsedFirstName { get; set; }

        public string? UsedLastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Guid? AddressId { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }
    }
}
