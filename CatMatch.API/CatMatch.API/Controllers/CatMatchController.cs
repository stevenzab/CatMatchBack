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
        public async Task<IActionResult> GetAllCat(CancellationToken cancellationToken)
        {
            var cat = await service.GetAllCatAsync(cancellationToken);
            return Ok(cat);
        }

        [HttpPost("VoteCat")]
        public async Task<IActionResult> VoteCat([FromBody] CatDto cat, CancellationToken cancellationToken)
        {
            var result = await service.VoteCatAsync(cat, cancellationToken);
            return CreatedAtAction(
                nameof(GetCatById),
                new { id = cat.Id },
                result);
        }
        [HttpGet("GetCatById/{id}")]
        public async Task<IActionResult> GetCatById(string id, CancellationToken cancellationToken)
        {
            var cat = await service.GetCatByIdAsync(id, cancellationToken);
            return Ok(cat);
        }
    }
}
