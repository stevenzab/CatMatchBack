using CatMatch.Domain.Dto;
using CatMatch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatMatch.Application.Services
{
    public interface ICatMatchService
    {
        Task<IEnumerable<CatDto>> GetAllCatAsync();
        Task<CatDto> VoteCat(CatDto cat);

        Task<CatDto> GetCatByIdAsync(string id);
    }
}
