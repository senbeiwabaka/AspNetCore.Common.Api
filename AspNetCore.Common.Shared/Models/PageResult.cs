namespace AspNetCore.Common.Shared.Models
{
    public sealed class PageResult<TModel>
        where TModel : BaseModel
    {
        public int TotalCount { get; set; }

        public IReadOnlyList<TModel> Items { get; set; } = new List<TModel>();
    }
}
