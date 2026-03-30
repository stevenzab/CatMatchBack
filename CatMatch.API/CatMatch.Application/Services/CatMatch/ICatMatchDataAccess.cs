using CatMatch.Domain.Models;

namespace CatMatch.Application.Services.CatMatch
{
    public interface ICatMatchDataAccess
    {
        IAsyncEnumerable<Cat> GetAllCatAsync(CancellationToken cancellationToken);
        Task VoteCatAsync(Cat cat, CancellationToken cancellationToken);
        Task<Cat> GetCatByIdAsync(string id, CancellationToken cancellationToken);
    }
}
