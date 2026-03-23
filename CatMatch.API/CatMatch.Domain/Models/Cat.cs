using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatMatch.Domain.Models
{
    public class Cat : RepositoryCollection
    {
        public string Url { get; set; }
    }
}
