using CatMatch.Domain.Models;
using CatMatch.Domain.SeedModels;
using CatMatch.Infrastructure.Common;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
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
            if (!File.Exists(seedFilePath))
            {
                return;
            }

            var json = await File.ReadAllTextAsync(seedFilePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };

            var parsed = JsonSerializer.Deserialize<SeedCat>(json, options);

            if (parsed?.Images != null && parsed.Images.Count > 0)
            {

                foreach (var img in parsed.Images)
                {
                    if (string.IsNullOrEmpty(img.Url))
                    {
                        continue;
                    }

                    var newCat = new Cat { Url = img.Url, OriginalId = img.OriginalId, Vote = 0 };
                    await repository.AddAsync(newCat);
                }
            }
        }
    }
}
