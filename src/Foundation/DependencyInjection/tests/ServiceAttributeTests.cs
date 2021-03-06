using FluentAssertions;
using Xunit;

namespace TTT.Foundation.DependencyInjection.Tests
{
    public class ServiceAttributeTests
  {
    [Fact]
    public void Constructor_WhenServiceTypeIsNotPassedAsAParameter_ShouldNotSetServiceTypeInServiceTypeProperty()
    {
      // Arrange, Act
      var attribute = new ServiceAttribute();

      // Assert
      attribute.ServiceType.Should().BeNull();
    }

    [Fact]
    public void Constructor_WhenServiceTypeIsPassedAsAParameter_ShouldSetServiceTypeInServiceTypeProperty()
    {
      // Arrange, Act
      var attribute = new ServiceAttribute(typeof(DiConfigurator));

      // Assert
      attribute.ServiceType.Name.Should().Be(nameof(DiConfigurator));
    }

    [Fact]
    public void Constructor_WhenServiceTypeAndLifetimeArePassedAsAParameters_ShouldSetServiceTypeAndLifetimeProperties()
    {
      // Arrange, Act
      var attribute = new ServiceAttribute(typeof(DiConfigurator), Lifetime.Singleton);

      // Assert
      attribute.ServiceType.Name.Should().Be(nameof(DiConfigurator));
      attribute.Lifetime.Should().Be(Lifetime.Singleton);
    }
  }
}
