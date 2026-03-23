using CatMatch.Domain.Settings;
using CatMatch.Infrastructure.Common;
using CatMatch.Infrastructure.Seeding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace CatMatch.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var mongodbSettings = configuration
               .GetSection("MongoDB")
               .Get<MongoSettings>();

            if (mongodbSettings == null)
                throw new InvalidOperationException("MongoDB settings are not configured properly.");

            var mongoClient = new MongoClient(mongodbSettings.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongodbSettings.DatabaseName);

            services.AddSingleton(mongoDatabase);
            services.AddScoped<IBaseRepository, BaseRepository>();
            services.AddScoped<ICatDbSeeder, CatDbSeeder>();
            return services;
        }
    }

    public static class DbSeederExtensions
    {
        public static async Task SeedDatabaseAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<ICatDbSeeder>();

            if (!await seeder.HasDataAsync())
                await seeder.SeedAsync();
        }
    }
}
