using CatMatch.Domain.Dto;
using CatMatch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatMatch.Application.Services.CatMatch
{
    public interface ICatMatchDataAccess
    {
        Task<IEnumerable<Cat>> GetAllCatAsync();
        Task VoteCat(Cat cat);
        Task<Cat> GetCatByIdAsync(string id);
    }
}
