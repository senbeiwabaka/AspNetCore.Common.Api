using AspNetCore.Common.Models;
using AspNetCore.Common.Shared;

namespace AspNetCore.Common.Domain
{
    public interface ISchoolReadonlyRepository : IReadonlyRepository<School>
    {
    }
}
