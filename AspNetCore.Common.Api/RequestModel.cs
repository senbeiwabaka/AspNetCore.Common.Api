using AspNetCore.Common.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Common.Api
{
    public sealed class RequestModel
    {
        [FromQuery]
        public List<Filter>? Filters { get; set; }

        [FromQuery]
        public Dictionary<string, string>? Ordering { get; set; }
    }
}
