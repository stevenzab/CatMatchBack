using CatMatch.Application.Services.CatMatch;
using CatMatch.Domain.Dto;
using CatMatch.Domain.MapDto;
using CatMatch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var cat = await service.GetAllCatAsync();

            var catDto = cat.Select(c => c.MapToDto()).ToList();

            return catDto;

        }
        public async Task<CatDto> VoteCat(CatDto catDto)
        {
            var cat = new Cat()
            {
                Url = catDto.Url,
                Vote = catDto.Vote
            };

            await service.VoteCat(cat);

            catDto.Id = cat.Id;

            return catDto;
        }

    }
}
