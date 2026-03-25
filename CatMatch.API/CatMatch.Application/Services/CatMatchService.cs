using CatMatch.Application.Services.CatMatch;
using CatMatch.Domain.Dto;
using CatMatch.Domain.MapDto;

namespace CatMatch.Application.Services
{
    public class CatMatchService : ICatMatchService
    {
        private readonly ICatMatchDataAccess service;

        public CatMatchService(ICatMatchDataAccess service)
        {
            this.service = service;
        }
        public async Task<IEnumerable<CatDto>> GetAllCatAsync()
        {
            var cats = await service.GetAllCatAsync();

            var catDtos = cats.Select(c => c.MapToDto()).ToList();

            return catDtos;
        }
        public async Task<CatDto> GetCatByIdAsync(string id)
        {
            var cat = await service.GetCatByIdAsync(id);

            if (cat == null)
                throw new KeyNotFoundException($"Chat avec l'ID {id} non trouvé");

            return cat.MapToDto();
        }
        public async Task<CatDto> VoteCatAsync(CatDto catDto)
        {
            var cat = await service.GetCatByIdAsync(catDto.Id);

            if (cat == null)
                throw new InvalidOperationException($"Chat avec l'ID {catDto.Id} non trouvé");

            cat.Vote = catDto.Vote;

            await service.VoteCat(cat);

            return catDto;
        }

    }
}
