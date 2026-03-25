using CatMatch.Domain.Models;
using MongoDB.Driver;

namespace CatMatch.Infrastructure.Common
{
    public class BaseRepository : IBaseRepository
    {
        private readonly IMongoDatabase database;

        public BaseRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        public IQueryable<T> AsQueryable<T>() => database.GetCollection<T>(typeof(T).Name).AsQueryable();

        public async Task AddAsync<T>(T entity) where T : RepositoryCollection
        {
            var collection = database.GetCollection<T>(typeof(T).Name);
            entity.Created = DateTime.Now;
            entity.Updated = DateTime.Now;
            await collection.InsertOneAsync(entity);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return database.GetCollection<T>(name);
        }

        public async Task<bool> UpdateVoteAsync(string id, int voteIncrement)
        {
            var collection = database.GetCollection<Cat>("Cat");

            var filter = Builders<Cat>.Filter.Eq(c => c.Id, id);
            var update = Builders<Cat>.Update
                .Inc(c => c.Vote, voteIncrement) 
                .Set(c => c.Updated, DateTime.Now);

            var result = await collection.UpdateOneAsync(filter, update);

            return result.ModifiedCount > 0;
        }

    }
}