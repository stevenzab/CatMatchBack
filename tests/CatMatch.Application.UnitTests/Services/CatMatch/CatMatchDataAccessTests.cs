using CatMatch.Application.Services.CatMatch;
using CatMatch.Domain.Models;
using CatMatch.Infrastructure.Common;
using Moq;

namespace CatMatch.Application.UnitTests.Services.CatMatch
{
    [TestClass]
    public class CatMatchDataAccessTests
    {
        private Mock<IBaseRepository> mockBaseRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            this.mockBaseRepository = new Mock<IBaseRepository>();
        }

        private CatMatchDataAccess CreateCatMatchDataAccess()
        {
            return new CatMatchDataAccess(this.mockBaseRepository.Object);
        }

        [TestMethod]
        public async Task VoteCat_UpdatesVoteSuccessfully()
        {
            // Arrange
            var cat = new Cat { Id = "1", Vote = 10 };
            this.mockBaseRepository
                .Setup(x => x.UpdateVoteAsync(cat.Id, cat.Vote, CancellationToken.None))
                .ReturnsAsync(true);

            var catMatchDataAccess = this.CreateCatMatchDataAccess();

            // Act
            await catMatchDataAccess.VoteCat(cat, CancellationToken.None);

            // Assert
            this.mockBaseRepository.Verify(x => x.UpdateVoteAsync(cat.Id, cat.Vote, CancellationToken.None), Times.Once);
        }

        [TestMethod]
        public async Task VoteCat_WithDifferentVoteValues()
        {
            // Arrange
            var cat = new Cat { Id = "5", Vote = 100 };
            this.mockBaseRepository
                .Setup(x => x.UpdateVoteAsync(cat.Id, cat.Vote, CancellationToken.None))
                .ReturnsAsync(true);

            var catMatchDataAccess = this.CreateCatMatchDataAccess();

            // Act
            await catMatchDataAccess.VoteCat(cat, CancellationToken.None);

            // Assert
            this.mockBaseRepository.Verify(x => x.UpdateVoteAsync("5", 100, CancellationToken.None), Times.Once);
        }

        [TestMethod]
        public async Task VoteCat_CallsRepositoryUpdateVoteAsync()
        {
            // Arrange
            var cat = new Cat { Id = "cat-123", Vote = 42 };
            this.mockBaseRepository
                .Setup(x => x.UpdateVoteAsync(cat.Id, cat.Vote, CancellationToken.None))
                .ReturnsAsync(true);

            var catMatchDataAccess = this.CreateCatMatchDataAccess();

            // Act
            await catMatchDataAccess.VoteCat(cat, CancellationToken.None);

            // Assert
            this.mockBaseRepository.Verify(
                x => x.UpdateVoteAsync("cat-123", 42, CancellationToken.None), 
                Times.Once);
        }
    }
}