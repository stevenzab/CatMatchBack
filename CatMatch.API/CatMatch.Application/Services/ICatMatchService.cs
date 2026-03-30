using CatMatch.Domain.Dto;

namespace CatMatch.Application.Services
{
    public interface ICatMatchService
    {
        IAsyncEnumerable<CatDto> GetAllCatAsync(CancellationToken cancellationToken);
        Task<CatDto> VoteCatAsync(CatDto cat, CancellationToken cancellationToken);
        Task<CatDto> GetCatByIdAsync(string id, CancellationToken cancellationToken);
    }
}
