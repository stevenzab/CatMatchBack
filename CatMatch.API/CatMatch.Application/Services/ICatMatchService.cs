using CatMatch.Domain.Dto;

namespace CatMatch.Application.Services
{
    public interface ICatMatchService
    {
        Task<IEnumerable<CatDto>> GetAllCatAsync(CancellationToken cancellationToken);
        Task<CatDto> VoteCatAsync(CatDto cat, CancellationToken cancellationToken);
        Task<CatDto> GetCatByIdAsync(string id, CancellationToken cancellationToken);
    }
}
