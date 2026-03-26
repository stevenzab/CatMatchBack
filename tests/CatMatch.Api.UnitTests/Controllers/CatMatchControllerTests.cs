using CatMatch.API.Controllers;
using CatMatch.Application.Services;
using CatMatch.Domain.Dto;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CatMatch.Api.UnitTests.Controllers
{
    [TestClass]
    public class CatMatchControllerTests
    {
        private Mock<ICatMatchService> mockService;
        private CatMatchController controller;

        [TestInitialize]
        public void Setup()
        {
            this.mockService = new Mock<ICatMatchService>();
            this.controller = new CatMatchController(mockService.Object);
        }

        [TestMethod]
        public async Task GetAllCat_ShouldReturnOkResult_WithCatsList()
        {
            // Arrange
            var catsExpected = new List<CatDto>
            {
                new CatDto { Id = "1", Url = "https://example.com/cat1.jpg", Vote = 10 },
                new CatDto { Id = "2", Url = "https://example.com/cat2.jpg", Vote = 5 }
            };

            mockService.Setup(s => s.GetAllCatAsync(CancellationToken.None))
                .ReturnsAsync(catsExpected);

            // Act
            var result = await controller.GetAllCat(CancellationToken.None);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedCats = okResult.Value as IEnumerable<CatDto>;
            Assert.IsNotNull(returnedCats);
            Assert.AreEqual(2, returnedCats.Count());

            mockService.Verify(s => s.GetAllCatAsync(CancellationToken.None), Times.Once);
        }

        [TestMethod]
        public async Task VoteCat_ShouldReturnCreatedAtActionResult()
        {
            // Arrange
            var catToVote = new CatDto { Id = "1", Url = "https://example.com/cat1.jpg", Vote = 10 };
            var catResult = new CatDto { Id = "1", Url = "https://example.com/cat1.jpg", Vote = 11 };

            mockService.Setup(s => s.VoteCatAsync(catToVote, CancellationToken.None))
                .ReturnsAsync(catResult);

            // Act
            var result = await controller.VoteCat(catToVote, CancellationToken.None);

            // Assert
            var createdResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual(nameof(CatMatchController.GetCatById), createdResult.ActionName);
            Assert.AreEqual(catToVote.Id, ((dynamic)createdResult.RouteValues)["id"]);

            var returnedCat = createdResult.Value as CatDto;
            Assert.IsNotNull(returnedCat);
            Assert.AreEqual(11, returnedCat.Vote);

            mockService.Verify(s => s.VoteCatAsync(catToVote, CancellationToken.None), Times.Once);
        }

        [TestMethod]
        public async Task GetCatById_ShouldReturnOkResult_WithSingleCat()
        {
            // Arrange
            var catId = "1";
            var catExpected = new CatDto { Id = "1", Url = "https://example.com/cat1.jpg", Vote = 10 };

            mockService.Setup(s => s.GetCatByIdAsync(catId, CancellationToken.None))
                .ReturnsAsync(catExpected);

            // Act
            var result = await controller.GetCatById(catId, CancellationToken.None);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedCat = okResult.Value as CatDto;
            Assert.IsNotNull(returnedCat);
            Assert.AreEqual(catId, returnedCat.Id);
            Assert.AreEqual("https://example.com/cat1.jpg", returnedCat.Url);
            Assert.AreEqual(10, returnedCat.Vote);

            mockService.Verify(s => s.GetCatByIdAsync(catId, CancellationToken.None), Times.Once);
        }

        [TestMethod]
        public async Task GetCatById_ShouldReturnOkResult_WithNullCat_WhenCatNotFound()
        {
            // Arrange
            var catId = "999";
            mockService.Setup(s => s.GetCatByIdAsync(catId, CancellationToken.None))
                .ReturnsAsync((CatDto)null);

            // Act
            var result = await controller.GetCatById(catId, CancellationToken.None);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNull(okResult.Value);

            mockService.Verify(s => s.GetCatByIdAsync(catId, CancellationToken.None), Times.Once);
        }
    }
}
