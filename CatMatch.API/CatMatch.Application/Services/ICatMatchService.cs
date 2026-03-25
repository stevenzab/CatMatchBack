using CatMatch.Domain.Dto;

namespace CatMatch.Application.Services
{
    public interface ICatMatchService
    {
        Task<IEnumerable<CatDto>> GetAllCatAsync();
        Task<CatDto> VoteCatAsync(CatDto cat);
        Task<CatDto> GetCatByIdAsync(string id);
    }
}
