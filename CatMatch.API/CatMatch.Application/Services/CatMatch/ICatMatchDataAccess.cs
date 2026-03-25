using CatMatch.Domain.Models;

namespace CatMatch.Application.Services.CatMatch
{
    public interface ICatMatchDataAccess
    {
        Task<IEnumerable<Cat>> GetAllCatAsync();
        Task VoteCat(Cat cat);
        Task<Cat> GetCatByIdAsync(string id);
    }
}
