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
        public async Task<IEnumerable<CatDto>> GetAllCatAsync(CancellationToken cancellationToken)
        {
            var cats = await service.GetAllCatAsync(cancellationToken);

            var catDtos = cats.Select(c => c.MapToDto());

            return catDtos;
        }
        public async Task<CatDto> GetCatByIdAsync(string id, CancellationToken cancellationToken)
        {
            var cat = await service.GetCatByIdAsync(id, cancellationToken);

            if (cat == null)
                throw new KeyNotFoundException($"Chat avec l'ID {id} non trouvé");

            return cat.MapToDto();
        }
        public async Task<CatDto> VoteCatAsync(CatDto catDto, CancellationToken cancellationToken)
        {
            var cat = await service.GetCatByIdAsync(catDto.Id, cancellationToken);

            if (cat == null)
                throw new InvalidOperationException($"Chat avec l'ID {catDto.Id} non trouvé");

            cat.Vote = catDto.Vote;

            await service.VoteCatAsync(cat, cancellationToken);

            return catDto;
        }

    }
}
