using AspNetCore.Common.Domain;
using AspNetCore.Common.Models;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Common.Api.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public sealed class CrudSchoolsController : ControllerBase
    {
        private readonly ISchoolRepository repository;
        private readonly IValidator<School> validator;

        public CrudSchoolsController(ISchoolRepository repository, IValidator<School> validator)
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<School>> Post(School school, CancellationToken cancellationToken)
        {
            var result = await validator.ValidateAsync(school, cancellationToken).ConfigureAwait(false);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            school = await repository.CreateAsync(school, cancellationToken).ConfigureAwait(false);

            return CreatedAtAction(nameof(Get), new { id = school.Id }, school);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(School school, CancellationToken cancellationToken)
        {
            var result = await validator.ValidateAsync(school, cancellationToken).ConfigureAwait(false);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            var existingEntity = await repository.GetByIdAsync(school.Id, cancellationToken).ConfigureAwait(false);

            if (existingEntity is null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Patch(Guid id, [FromBody] JsonPatchDocument<School> patchDoc, CancellationToken cancellationToken)
        {
            if (patchDoc is not null)
            {
                var existingEntity = await repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

                if (existingEntity is null)
                {
                    return NotFound();
                }

                patchDoc.ApplyTo(existingEntity, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var school = await repository.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

            if (school is null)
            {
                return NotFound();
            }

            await repository.DeleteAsync(id, cancellationToken).ConfigureAwait(false);

            return NoContent();
        }
    }
}
