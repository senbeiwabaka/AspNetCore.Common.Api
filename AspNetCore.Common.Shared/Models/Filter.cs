namespace AspNetCore.Common.Shared.Models
{
    public sealed class Filter
    {
        public string Property { get; set; } = default!;

        public FilterType FilterType { get; set; }

        public string? Value { get; set; }
    }
}
