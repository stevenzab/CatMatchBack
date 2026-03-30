using CatMatch.Application.Services;
using CatMatch.Domain.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CatMatch.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatMatchController : ControllerBase
    {
        private readonly ICatMatchService service;

        public CatMatchController(ICatMatchService service)
        {
            this.service = service;
        }

        [HttpGet("GetAllCat")]
        public async IAsyncEnumerable<CatDto> GetAllCat(CancellationToken cancellationToken)
        {
            await foreach(var cat in service.GetAllCatAsync(cancellationToken))
            {
                yield return cat;
            }
        }

        // return un chat directement avec le vote de la base, mettre en PUT
        [HttpPost("VoteCat")]
        public async Task<IActionResult> VoteCat([FromBody] CatDto cat, CancellationToken cancellationToken)
        {
            var result = await service.VoteCatAsync(cat, cancellationToken);
            return CreatedAtAction(
                nameof(GetCatById),
                new { id = cat.Id },
                result);
        }

        // endpoint votecatbyID (body tient un champs enumerable like / dislike)

        [HttpGet("GetCatById/{id}")]
        public async Task<IActionResult> GetCatById(string id, CancellationToken cancellationToken)
        {
            var cat = await service.GetCatByIdAsync(id, cancellationToken);
            return Ok(cat);
        }
    }
}
