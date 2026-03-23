using CatMatch.Domain.Dto;
using CatMatch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
                Url = source.Url,
                Vote = source.Vote
            };
        }
    }
}
