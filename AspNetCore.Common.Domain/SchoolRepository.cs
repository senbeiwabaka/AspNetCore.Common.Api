using AspNetCore.Common.Data;
using AspNetCore.Common.Models;

namespace AspNetCore.Common.Domain
{
    public sealed class SchoolRepository : Repository<School>, ISchoolRepository
    {
        public SchoolRepository(AppDbContext context)
            : base(context)
        {
        }
    }
}
