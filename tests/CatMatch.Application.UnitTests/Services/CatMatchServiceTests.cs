using CatMatch.Application.Services;
using CatMatch.Application.Services.CatMatch;
using CatMatch.Domain.Dto;
using CatMatch.Domain.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


//check fluent assertions
namespace CatMatch.Application.UnitTests.Services
{
    [TestClass]
    public class CatMatchServiceTests
    {
        private MockRepository mockRepository;

        private Mock<ICatMatchDataAccess> mockCatMatchDataAccess;

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);

            this.mockCatMatchDataAccess = this.mockRepository.Create<ICatMatchDataAccess>();
        }

        private CatMatchService CreateService()
        {
            return new CatMatchService(
                this.mockCatMatchDataAccess.Object);
        }
        private static async IAsyncEnumerable<Cat> GetAsyncEnumerable(List<Cat> items)
        {
            foreach (var item in items)
            {
                yield return item;
            }
        }

        [TestMethod]
        public async Task GetAllCatAsync_ShouldReturnMappedCats()
        {
            // Arrange
            var service = this.CreateService();
            var catsFromDataAccess = new List<Cat>
            {
                new Cat { Id = "1", Url = "https://example.com/cat1.jpg", Vote = 10, OriginalId = "orig1" },
                new Cat { Id = "2", Url = "https://example.com/cat2.jpg", Vote = 5, OriginalId = "orig2" }
            };

            this.mockCatMatchDataAccess
                .Setup(x => x.GetAllCatAsync(CancellationToken.None))
                .Returns(GetAsyncEnumerable(catsFromDataAccess));

            // Act
            var result = service.GetAllCatAsync(CancellationToken.None);

            // Assert
            var returnedCats = new List<CatDto>();
            await foreach (var cat in result)
            {
                returnedCats.Add(cat);
            }

            Assert.IsNotNull(result);
            Assert.AreEqual(2, returnedCats.Count);
            Assert.AreEqual("1", returnedCats[0].Id);
            Assert.AreEqual("https://example.com/cat1.jpg", returnedCats[0].Url);
            Assert.AreEqual(10, returnedCats[0].Vote);
            Assert.AreEqual("2", returnedCats[1].Id);
            Assert.AreEqual(5, returnedCats[1].Vote);

            this.mockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetAllCatAsync_ShouldReturnEmptyList_WhenNoCats()
        {
            // Arrange
            var service = this.CreateService();
            var catsFromDataAccess = new List<Cat>();

            this.mockCatMatchDataAccess
                .Setup(x => x.GetAllCatAsync(CancellationToken.None))
                .Returns(GetAsyncEnumerable(catsFromDataAccess));

            // Act
            var result = service.GetAllCatAsync(CancellationToken.None);

            // Assert
            var returnedCats = new List<CatDto>();
            await foreach (var cat in result)
            {
                returnedCats.Add(cat);
            }

            Assert.IsNotNull(result);
            Assert.AreEqual(0, returnedCats.Count);

            this.mockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetCatByIdAsync_ShouldReturnMappedCat_WhenCatExists()
        {
            // Arrange
            var service = this.CreateService();
            string id = "1";
            var catFromDataAccess = new Cat 
            { 
                Id = "1", 
                Url = "https://example.com/cat1.jpg", 
                Vote = 10, 
                OriginalId = "orig1" 
            };

            this.mockCatMatchDataAccess
                .Setup(x => x.GetCatByIdAsync(id, CancellationToken.None))
                .ReturnsAsync(catFromDataAccess);

            // Act
            var result = await service.GetCatByIdAsync(id, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("1", result.Id);
            Assert.AreEqual("https://example.com/cat1.jpg", result.Url);
            Assert.AreEqual(10, result.Vote);

            this.mockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task VoteCatAsync_ShouldUpdateCatVote_WhenCatExists()
        {
            // Arrange
            var service = this.CreateService();
            var catDto = new CatDto { Id = "1", Url = "https://example.com/cat1.jpg", Vote = 15 };
            var existingCat = new Cat 
            { 
                Id = "1", 
                Url = "https://example.com/cat1.jpg", 
                Vote = 10, 
                OriginalId = "orig1" 
            };

            this.mockCatMatchDataAccess
                .Setup(x => x.GetCatByIdAsync("1", CancellationToken.None))
                .ReturnsAsync(existingCat);

            this.mockCatMatchDataAccess
                .Setup(x => x.VoteCatAsync(It.IsAny<Cat>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            // Act
            var result = await service.VoteCatAsync(catDto, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("1", result.Id);
            Assert.AreEqual(15, result.Vote);

            this.mockCatMatchDataAccess.Verify(
                x => x.GetCatByIdAsync("1", CancellationToken.None), 
                Times.Once);

            this.mockCatMatchDataAccess.Verify(
                x => x.VoteCatAsync(It.Is<Cat>(c => c.Vote == 15), CancellationToken.None), 
                Times.Once);

            this.mockRepository.VerifyAll();
        }
    }
}
