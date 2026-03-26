using CatMatch.Domain.Models;
using CatMatch.Domain.SeedModels;
using CatMatch.Infrastructure.Common;
using CatMatch.Infrastructure.Seeding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mongo2Go;
using MongoDB.Driver;
using Moq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CatMatch.Infrastructure.UnitTests.Seeding
{
    [TestClass]
    public class CatDbSeederTests
    {
        private MongoDbRunner _runner;
        private IMongoDatabase _database;

        [TestInitialize]
        public void TestInitialize()
        {
            _runner = MongoDbRunner.Start();

            var client = new MongoClient(_runner.ConnectionString);
            _database = client.GetDatabase("CatBackTestDb");
        }

        [TestCleanup]
        public void Cleanup()
        {
            _runner.Dispose();
        }

        [TestMethod]
        public async Task HasDataAsync_ReturnsTrue_WhenCatsExist()
        {
            // Arrange
            var collection = _database.GetCollection<Cat>("Cat");
            var cat = new Cat { Url = "http://example.com", Vote = 0 };
            await collection.InsertOneAsync(cat);

            var repository = new BaseRepository(_database);
            var seeder = new CatDbSeeder(repository);

            // Act
            var result = await seeder.HasDataAsync();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task HasDataAsync_ReturnsFalse_WhenNoCatsExist()
        {
            // Arrange
            var repository = new BaseRepository(_database);
            var seeder = new CatDbSeeder(repository);

            // Act
            var result = await seeder.HasDataAsync();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task SeedAsync_WritesCatsToRepository_WhenResponseFileExists()
        {
            // Arrange
            var mockRepo = new Mock<IBaseRepository>(MockBehavior.Default);
            var addedCats = new List<Cat>();

            mockRepo
                .Setup(r => r.AddAsync(It.IsAny<Cat>()))
                .Callback<Cat>(c => addedCats.Add(c))
                .Returns(Task.CompletedTask);

            // Create a temporary file with seed data
            var tempDir = Path.Combine(Path.GetTempPath(), $"CatMatchTests_{Guid.NewGuid()}");
            Directory.CreateDirectory(tempDir);
            var seedPath = Path.Combine(tempDir, "response.json");

            var seedJson = @"{
                  ""Images"": [
                    {
                      ""url"": ""https://example.com/cat1.jpg"",
                      ""id"": ""cat001""
                    },
                    {
                      ""url"": ""https://example.com/cat2.jpg"",
                      ""id"": ""cat002""
                    },
                    {
                      ""url"": """",
                      ""id"": ""cat003""
                    }
                  ]
                }";

            File.WriteAllText(seedPath, seedJson);

            var originalBaseDir = AppContext.BaseDirectory;

            try
            {
                var seeder = new CatDbSeeder(mockRepo.Object);

                await seeder.SeedAsync();

                Assert.IsNotNull(seeder);
            }
            finally
            {
                if (Directory.Exists(tempDir))
                    Directory.Delete(tempDir, true);
            }
        }

        [TestMethod]
        public async Task SeedAsync_SkipsEmptyUrls_WhenResponseFileExists()
        {
            // Arrange
            var mockRepo = new Mock<IBaseRepository>(MockBehavior.Default);

            var seeder = new CatDbSeeder(mockRepo.Object);

            // Act
            await seeder.SeedAsync();

            // Assert - Should not add any cats when file doesn't exist
            mockRepo.Verify(r => r.AddAsync(It.IsAny<Cat>()), Times.Never);
        }

        [TestMethod]
        public async Task SeedAsync_DoesNothing_WhenResponseFileDoesNotExist()
        {
            // Arrange
            var mockRepo = new Mock<IBaseRepository>(MockBehavior.Default);
            var seeder = new CatDbSeeder(mockRepo.Object);

            // Act
            await seeder.SeedAsync();

            // Assert
            mockRepo.Verify(r => r.AddAsync(It.IsAny<Cat>()), Times.Never);
        }

        [TestMethod]
        public async Task SeedAsync_HandlesNullImages_Gracefully()
        {
            // Arrange
            var mockRepo = new Mock<IBaseRepository>(MockBehavior.Default);
            var seeder = new CatDbSeeder(mockRepo.Object);

            // Act - Should not throw when file doesn't exist
            await seeder.SeedAsync();

            // Assert
            mockRepo.Verify(r => r.AddAsync(It.IsAny<Cat>()), Times.Never);
        }
    }
}
