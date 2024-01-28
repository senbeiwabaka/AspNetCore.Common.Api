namespace AspNetCore.Common.Shared.Models
{
    public abstract class BaseModel
    {
        public Guid Id { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }
    }
}
