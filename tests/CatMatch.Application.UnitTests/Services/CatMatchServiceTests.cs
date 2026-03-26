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
                .Setup(x => x.GetAllCatAsync())
                .ReturnsAsync(catsFromDataAccess);

            // Act
            var result = await service.GetAllCatAsync();

            // Assert
            Assert.IsNotNull(result);
            var resultList = result.ToList();
            Assert.AreEqual(2, resultList.Count);
            Assert.AreEqual("1", resultList[0].Id);
            Assert.AreEqual("https://example.com/cat1.jpg", resultList[0].Url);
            Assert.AreEqual(10, resultList[0].Vote);
            Assert.AreEqual("2", resultList[1].Id);
            Assert.AreEqual(5, resultList[1].Vote);

            this.mockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetAllCatAsync_ShouldReturnEmptyList_WhenNoCats()
        {
            // Arrange
            var service = this.CreateService();
            var catsFromDataAccess = new List<Cat>();

            this.mockCatMatchDataAccess
                .Setup(x => x.GetAllCatAsync())
                .ReturnsAsync(catsFromDataAccess);

            // Act
            var result = await service.GetAllCatAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());

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
                .Setup(x => x.GetCatByIdAsync(id))
                .ReturnsAsync(catFromDataAccess);

            // Act
            var result = await service.GetCatByIdAsync(id);

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
                .Setup(x => x.GetCatByIdAsync("1"))
                .ReturnsAsync(existingCat);

            this.mockCatMatchDataAccess
                .Setup(x => x.VoteCat(It.IsAny<Cat>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await service.VoteCatAsync(catDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("1", result.Id);
            Assert.AreEqual(15, result.Vote);

            this.mockCatMatchDataAccess.Verify(
                x => x.GetCatByIdAsync("1"), 
                Times.Once);

            this.mockCatMatchDataAccess.Verify(
                x => x.VoteCat(It.Is<Cat>(c => c.Vote == 15)), 
                Times.Once);

            this.mockRepository.VerifyAll();
        }
    }
}
