using CatMatch.Domain.Models;
using CatMatch.Infrastructure.Common;
using MongoDB.Driver.Linq;

namespace CatMatch.Application.Services.CatMatch
{
    public class CatMatchDataAccess : ICatMatchDataAccess
    {
        private readonly IBaseRepository baseRepository;

        public CatMatchDataAccess(IBaseRepository baseRepository)
        {
            this.baseRepository = baseRepository;
        }

        //use IASYNCENUMERABLE check "gère la mémoire tt seul possède un await" "toList stock en mémoire"
        public IAsyncEnumerable<Cat> GetAllCatAsync(CancellationToken cancellationToken)
        {
           return baseRepository.AsQueryable<Cat>().OrderByDescending(x => x.Vote).ToAsyncEnumerable();
        }

        public async Task VoteCatAsync(Cat cat, CancellationToken cancellationToken)
        {
            await baseRepository.UpdateVoteAsync(cat.Id, cat.Vote, cancellationToken);
        }

        public async Task<Cat> GetCatByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await baseRepository.AsQueryable<Cat>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}
