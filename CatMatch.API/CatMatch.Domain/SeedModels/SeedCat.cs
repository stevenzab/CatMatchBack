using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CatMatch.Domain.SeedModels
{
    public class SeedCat
    {
        public List<CatImage> Images { get; set; }
    }
}
