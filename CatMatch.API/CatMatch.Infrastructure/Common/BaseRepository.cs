using CatMatch.Domain.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}