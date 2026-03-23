using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatMatch.Infrastructure.Seeding
{
    public interface ICatDbSeeder
    {
        Task<bool> HasDataAsync();

        Task SeedAsync();
    }
}
