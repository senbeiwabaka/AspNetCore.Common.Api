using AspNetCore.Common.Data;
using AspNetCore.Common.Models;

namespace AspNetCore.Common.Domain
{
    public sealed class SchoolReadonlyRepository : ReadonlyRepository<School>, ISchoolReadonlyRepository
    {
        public SchoolReadonlyRepository(AppDbContext context)
            : base(context)
        {
        }
    }
}
