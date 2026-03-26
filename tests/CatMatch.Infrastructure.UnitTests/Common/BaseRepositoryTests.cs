using CatMatch.Domain.Models;
using CatMatch.Infrastructure.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CatMatch.Infrastructure.UnitTests.Common
{
    [TestClass]
    public class BaseRepositoryTests
    {
        private MockRepository mockRepository;
        private Mock<IMongoDatabase> mockMongoDatabase;
        private Mock<IMongoCollection<Cat>> mockMongoCollectionCat;

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            this.mockMongoDatabase = this.mockRepository.Create<IMongoDatabase>();
            this.mockMongoCollectionCat = this.mockRepository.Create<IMongoCollection<Cat>>();
        }

        private BaseRepository CreateBaseRepository()
        {
            return new BaseRepository(
                this.mockMongoDatabase.Object);
        }

        [TestMethod]
        public void AsQueryable_CallsGetCollection_WithCorrectTypeName()
        {
            // Arrange
            mockMongoDatabase
                .Setup(x => x.GetCollection<Cat>(typeof(Cat).Name))
                .Throws(new ArgumentNullException());

            var baseRepository = this.CreateBaseRepository();

            // Act & Assert
            try
            {
                baseRepository.AsQueryable<Cat>();
                Assert.Fail("Expected ArgumentNullException");
            }
            catch (ArgumentNullException)
            {
                mockMongoDatabase.Verify(x => x.GetCollection<Cat>(typeof(Cat).Name), Times.Once());
            }
        }

        [TestMethod]
        public async Task AddAsync_InsertsEntityWithTimestamps_WhenEntityIsValid()
        {
            // Arrange
            var entity = new Cat 
            { 
                Url = "http://example.com",
                OriginalId = "123",
                Vote = 0
            };

            mockMongoDatabase
                .Setup(x => x.GetCollection<Cat>(typeof(Cat).Name))
                .Returns(mockMongoCollectionCat.Object);

            mockMongoCollectionCat
                .Setup(x => x.InsertOneAsync(It.IsAny<Cat>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Callback<Cat, InsertOneOptions, CancellationToken>((cat, options, ct) =>
                {
                    Assert.IsNotNull(cat.Created);
                    Assert.IsNotNull(cat.Updated);
                });

            var baseRepository = this.CreateBaseRepository();

            // Act
            await baseRepository.AddAsync(entity);

            // Assert
            mockMongoDatabase.Verify(x => x.GetCollection<Cat>(typeof(Cat).Name), Times.Once());
            mockMongoCollectionCat.Verify(
                x => x.InsertOneAsync(It.IsAny<Cat>(), It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()), 
                Times.Once());
        }

        [TestMethod]
        public void GetCollection_ReturnsCorrectCollection_WithGivenName()
        {
            // Arrange
            var collectionName = "Cat";
            mockMongoDatabase
                .Setup(x => x.GetCollection<Cat>(collectionName))
                .Returns(mockMongoCollectionCat.Object);

            var baseRepository = this.CreateBaseRepository();

            // Act
            var result = baseRepository.GetCollection<Cat>(collectionName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(mockMongoCollectionCat.Object, result);
            mockMongoDatabase.Verify(x => x.GetCollection<Cat>(collectionName), Times.Once());
        }

        [TestMethod]
        public async Task UpdateVoteAsync_UpdatesCatVote_AndReturnsTrue_WhenCatExists()
        {
            // Arrange
            var catId = "507f1f77bcf86cd799439011";
            var voteIncrement = 5;
            var mockUpdateResult = new Mock<UpdateResult>();
            mockUpdateResult.Setup(x => x.ModifiedCount).Returns(1L);

            mockMongoDatabase
                .Setup(x => x.GetCollection<Cat>("Cat"))
                .Returns(mockMongoCollectionCat.Object);

            mockMongoCollectionCat
                .Setup(x => x.UpdateOneAsync(
                    It.IsAny<FilterDefinition<Cat>>(),
                    It.IsAny<UpdateDefinition<Cat>>(),
                    null,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockUpdateResult.Object);

            var baseRepository = this.CreateBaseRepository();

            // Act
            var result = await baseRepository.UpdateVoteAsync(catId, voteIncrement, CancellationToken.None);

            // Assert
            Assert.IsTrue(result);
            mockMongoDatabase.Verify(x => x.GetCollection<Cat>("Cat"), Times.Once());
            mockMongoCollectionCat.Verify(
                x => x.UpdateOneAsync(
                    It.IsAny<FilterDefinition<Cat>>(),
                    It.IsAny<UpdateDefinition<Cat>>(),
                    null,
                    It.IsAny<CancellationToken>()),
                Times.Once());
        }

        [TestMethod]
        public async Task UpdateVoteAsync_ReturnsFalse_WhenCatDoesNotExist()
        {
            // Arrange
            var catId = "507f1f77bcf86cd799439011";
            var voteIncrement = 5;
            var mockUpdateResult = new Mock<UpdateResult>();
            mockUpdateResult.Setup(x => x.ModifiedCount).Returns(0);

            mockMongoDatabase
                .Setup(x => x.GetCollection<Cat>("Cat"))
                .Returns(mockMongoCollectionCat.Object);

            mockMongoCollectionCat
                .Setup(x => x.UpdateOneAsync(
                    It.IsAny<FilterDefinition<Cat>>(),
                    It.IsAny<UpdateDefinition<Cat>>(),
                    null,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockUpdateResult.Object);

            var baseRepository = this.CreateBaseRepository();

            // Act
            var result = await baseRepository.UpdateVoteAsync(catId, voteIncrement, CancellationToken.None);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
