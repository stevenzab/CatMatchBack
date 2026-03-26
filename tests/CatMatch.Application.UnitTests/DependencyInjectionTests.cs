using CatMatch.Application;
using CatMatch.Application.Services;
using CatMatch.Application.Services.CatMatch;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CatMatch.Application.UnitTests
{
    [TestClass]
    public class DependencyInjectionTests
    {

        [TestMethod]
        public void AddApplication_RegistersCatMatchServiceAsScoped()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddApplication();

            // Assert
            var descriptor = services.FirstOrDefault(sd => sd.ServiceType == typeof(ICatMatchService));
            Assert.IsNotNull(descriptor);
            Assert.AreEqual(ServiceLifetime.Scoped, descriptor.Lifetime);
            Assert.AreEqual(typeof(CatMatchService), descriptor.ImplementationType);
        }

        [TestMethod]
        public void AddApplication_RegistersCatMatchDataAccessAsScoped()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddApplication();

            // Assert
            var descriptor = services.FirstOrDefault(sd => sd.ServiceType == typeof(ICatMatchDataAccess));
            Assert.IsNotNull(descriptor);
            Assert.AreEqual(ServiceLifetime.Scoped, descriptor.Lifetime);
            Assert.AreEqual(typeof(CatMatchDataAccess), descriptor.ImplementationType);
        }

        [TestMethod]
        public void AddApplication_ReturnsIServiceCollection()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = services.AddApplication();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(services, result);
        }

        [TestMethod]
        public void AddApplication_RegistersBothServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddApplication();

            // Assert
            var descriptors = services.ToList();
            Assert.AreEqual(2, descriptors.Count);

            var serviceTypes = descriptors.Select(d => d.ServiceType).ToList();
            Assert.IsTrue(serviceTypes.Contains(typeof(ICatMatchService)));
            Assert.IsTrue(serviceTypes.Contains(typeof(ICatMatchDataAccess)));
        }

        [TestMethod]
        public void AddApplication_RegistersCorrectImplementations()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddApplication();

            // Assert
            var serviceDescriptor = services.FirstOrDefault(sd => sd.ServiceType == typeof(ICatMatchService));
            Assert.IsNotNull(serviceDescriptor);
            Assert.AreEqual(typeof(CatMatchService), serviceDescriptor.ImplementationType);

            var dataAccessDescriptor = services.FirstOrDefault(sd => sd.ServiceType == typeof(ICatMatchDataAccess));
            Assert.IsNotNull(dataAccessDescriptor);
            Assert.AreEqual(typeof(CatMatchDataAccess), dataAccessDescriptor.ImplementationType);
        }
    }
}
