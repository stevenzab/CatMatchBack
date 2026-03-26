using CatMatch.Domain.MapDto;
using CatMatch.Domain.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CatMatch.Domain.UnitTests.MapDto
{
    [TestClass]
    public class MappingTests
    {
        [TestMethod]
        public void MapToDto_FullCat_MapsAllProperties()
        {
            var source = new Cat
            {
                Id = "mongodb-id-123",
                Url = "http://example.cat",
                Vote = 2
            };

            var dto = source.MapToDto();

            Assert.IsNotNull(dto);
            Assert.AreEqual("mongodb-id-123", dto.Id);
            Assert.AreEqual("http://example.cat", dto.Url);
            Assert.AreEqual(2, dto.Vote);
        }

        [TestMethod]
        public void MapToDto_CatWithDifferentOriginalId_MapsCorrectly()
        {
            var source = new Cat
            {
                Id = "mongodb-id-456",
                Url = "http://example2.cat",
                Vote = 5
            };

            var dto = source.MapToDto();

            Assert.IsNotNull(dto);
            Assert.AreEqual("mongodb-id-456", dto.Id);
            Assert.AreEqual("http://example2.cat", dto.Url);
            Assert.AreEqual(5, dto.Vote);
        }

        [TestMethod]
        public void MapToDto_NullCat_ReturnsNull()
        {
            Cat source = null;

            var dto = source.MapToDto();

            Assert.IsNull(dto);
        }
    }
}
