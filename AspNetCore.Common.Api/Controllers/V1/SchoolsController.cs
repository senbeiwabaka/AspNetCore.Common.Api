using AspNetCore.Common.Api.Validations.V1;
using AspNetCore.Common.Domain;
using AspNetCore.Common.Models;
using AspNetCore.Common.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Common.Api.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public sealed class SchoolsController : ControllerBase
    {
        private readonly ISchoolReadonlyRepository repository;
        private readonly RequestModelValidator<School> validator;

        public SchoolsController(ISchoolReadonlyRepository repository, RequestModelValidator<School> validator)
        {
            this.repository = repository;
            this.validator = validator;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<School>> Get(Guid id, CancellationToken cancellationToken)
        {
            var school = await repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

            if (school is null)
            {
                return NotFound();
            }

            return Ok(school);
        }

        [HttpGet("gets/{page}/{pageSize}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PageResult<School>>> GetList(
            int page,
            int pageSize,
            [FromQuery] RequestModel request,
            CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken).ConfigureAwait(false);

            if (!validationResult.IsValid)
            {
                return BadRequest();
            }

            return Ok(await repository.GetListAsync(
                page,
                pageSize,
                request.Filters,
                request.Ordering,
                cancellationToken)
                .ConfigureAwait(false));
        }
    }
}
