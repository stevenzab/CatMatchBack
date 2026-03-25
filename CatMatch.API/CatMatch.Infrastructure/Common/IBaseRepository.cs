using CatMatch.Domain.Models;
using MongoDB.Driver;

namespace CatMatch.Infrastructure.Common
{
    public interface IBaseRepository
    {
        Task AddAsync<T>(T entity) where T : RepositoryCollection;

        IQueryable<T> AsQueryable<T>();

        IMongoCollection<T> GetCollection<T>(string name);

        Task<bool> UpdateVoteAsync(string id, int voteIncrement);
    }
}
