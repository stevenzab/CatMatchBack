using CatMatch.Domain.Models;

namespace CatMatch.Application.Services.CatMatch
{
    public interface ICatMatchDataAccess
    {
        Task<IEnumerable<Cat>> GetAllCatAsync(CancellationToken cancellationToken);
        Task VoteCat(Cat cat, CancellationToken cancellationToken);
        Task<Cat> GetCatByIdAsync(string id, CancellationToken cancellationToken);
    }
}
