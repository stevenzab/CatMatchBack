namespace CatMatch.Infrastructure.Seeding
{
    public interface ICatDbSeeder
    {
        Task<bool> HasDataAsync();

        Task SeedAsync();
    }
}
