using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace ToDoList.Domain.UnitTests
{
    /// <summary>
    /// Defines the <see cref="DependencyInjectionTest" />.
    /// </summary>
    public class DependencyInjectionTest
    {
        /// <summary>
        /// The Constructor_ServiceCollectionIsNotNull_AddDomain.
        /// </summary>
        [Fact]
        public void Constructor_ServiceCollectionIsNotNull_AddDomain()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = DependencyInjection.AddDomain(services);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ServiceCollection>(result);
            Assert.True(result.Count > 0);
        }

        /// <summary>
        /// The Constructor_ServiceCollectionIsNull_ThrowsArgumentNullException.
        /// </summary>
        [Fact]
        public void Constructor_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                DependencyInjection.AddDomain(null));
        }
    }
}
