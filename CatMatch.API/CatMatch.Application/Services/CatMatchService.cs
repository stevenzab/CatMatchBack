using CatMatch.Application.Services.CatMatch;
using CatMatch.Domain.Dto;
using CatMatch.Domain.MapDto;
using System.Runtime.CompilerServices;

namespace CatMatch.Application.Services
{
    public class CatMatchService : ICatMatchService
    {
        private readonly ICatMatchDataAccess service;

        public CatMatchService(ICatMatchDataAccess service)
        {
            this.service = service;
        }
        public async IAsyncEnumerable<CatDto> GetAllCatAsync(CancellationToken cancellationToken)
        {
            await foreach (var cat in service.GetAllCatAsync(cancellationToken))
            {
                yield return cat.MapToDto();
            }
        }
        public async Task<CatDto> GetCatByIdAsync(string id, CancellationToken cancellationToken)
        {
            var cat = await service.GetCatByIdAsync(id, cancellationToken);

            //catch not found
            //fortement recommandé de crée des exceptions personnalisées / crée une class qui hérite de IException
            //pour transformer en 404 crée un filtre supp.
            //regarder les exceptions doc
            if (cat == null)
                throw new KeyNotFoundException($"Chat avec l'ID {id} non trouvé");
            //catch l'exception et appelée route 404 not found controller

            return cat.MapToDto();
        }
        public async Task<CatDto> VoteCatAsync(CatDto catDto, CancellationToken cancellationToken)
        {
            var cat = await service.GetCatByIdAsync(catDto.Id, cancellationToken);

            //même histoire getbyid
            if (cat == null)
                throw new InvalidOperationException($"Chat avec l'ID {catDto.Id} non trouvé");


            cat.Vote = catDto.Vote;
            // bool simple qui récupère déja la donnée pas besoin d'use getcatbyID peut se reposer dessus
            await service.VoteCatAsync(cat, cancellationToken);

            return catDto;
        }

    }
}
