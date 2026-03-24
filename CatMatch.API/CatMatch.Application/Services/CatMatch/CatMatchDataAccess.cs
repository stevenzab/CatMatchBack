using CatMatch.Domain.Dto;
using CatMatch.Domain.Models;
using CatMatch.Infrastructure.Common;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatMatch.Application.Services.CatMatch
{
    public class CatMatchDataAccess : ICatMatchDataAccess
    {
        private readonly IBaseRepository baseRepository;

        public CatMatchDataAccess(IBaseRepository baseRepository)
        {
            this.baseRepository = baseRepository;
        }

        public async Task<IEnumerable<Cat>> GetAllCatAsync()
        {
            return await baseRepository.AsQueryable<Cat>().OrderByDescending(x => x.Vote).ToListAsync();
        }

        public async Task VoteCat(Cat cat)
        {
            await baseRepository.UpdateVoteAsync(cat.Id, cat.Vote);
        }

        public async Task<Cat> GetCatByIdAsync(string id)
        {
            return await baseRepository.AsQueryable<Cat>().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
