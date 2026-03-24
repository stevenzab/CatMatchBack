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
        public async Task<IActionResult> GetAllCat()
        {
            var cat = await service.GetAllCatAsync();
            return Ok(cat);
        }

        [HttpPost("VoteCat")]
        public async Task<IActionResult> VoteCat([FromBody] CatDto cat)
        {
            var result = await service.VoteCat(cat);
            return Ok(result);
        }
    }
}
