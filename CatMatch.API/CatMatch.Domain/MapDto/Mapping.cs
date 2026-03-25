using CatMatch.Domain.Dto;
using CatMatch.Domain.Models;

namespace CatMatch.Domain.MapDto
{
    public static class Mapping
    {
        public static CatDto MapToDto(this Cat source)
        {

            if (source == null)
                return null;

            return new CatDto
            {
                Id = source.Id,
                Url = source.Url,
                Vote = source.Vote
            };
        }
    }
}
