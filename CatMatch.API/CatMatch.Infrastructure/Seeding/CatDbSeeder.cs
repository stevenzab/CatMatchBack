using CatMatch.Domain.Models;
using CatMatch.Domain.SeedModels;
using CatMatch.Infrastructure.Common;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CatMatch.Infrastructure.Seeding
{
    public class CatDbSeeder : ICatDbSeeder
    {
        private readonly IBaseRepository repository;
        private readonly string seedFilePath;

        public CatDbSeeder(IBaseRepository repository)
        {
            this.repository = repository;
            seedFilePath = Path.Combine(AppContext.BaseDirectory, "response.json");
        }

        public async Task<bool> HasDataAsync()
        {
            var hasPlayers = await repository.AsQueryable<Cat>().AnyAsync();
            return hasPlayers;
        }

        public async Task SeedAsync()
        {
            if (await HasDataAsync())
            {
                return;
            }

            if (File.Exists(seedFilePath))
            {
                return;
            }

            var json = await File.ReadAllTextAsync(seedFilePath);
            var parsed = System.Text.Json.JsonSerializer.Deserialize<SeedCat>(json, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (parsed?.Images != null)
            {
                foreach (var img in parsed.Images)
                {
                    await repository.AddAsync(new Cat { Url = img.Url });
                }
            }
        }
    }
}
