using System;
using System.IO;
using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace TTT.Foundation.DependencyInjection.Tests
{
    using ServiceCollectionExtensions = ServiceCollectionExtensions;

  public class ServiceCollectionExtensionsTests
  {
      [Fact]
    public void AddClassesWithServiceAttribute_TypesWithServiceRegistration_ServicesCollectionIsConfigured()
    {
      // Arrange
      IServiceCollection serviceCollection = new ServiceCollection();

      var interfaceType = typeof(AddClassesWithServiceAttributeTestClasses.InterfaceClass);
      var implementationType = typeof(AddClassesWithServiceAttributeTestClasses.ImplementationClass);
      var selfRegistrationType = typeof(AddClassesWithServiceAttributeTestClasses.SelfRegistrationClass);
      var selfRegistrationAbstractType = typeof(AddClassesWithServiceAttributeTestClasses.AbstractSelfRegistrationClass);
      var selfRegistrationAbstractGenericType = typeof(AddClassesWithServiceAttributeTestClasses.AbstractGenericSelfRegistrationClass<object>);
      var selfRegistrationGenericType = typeof(AddClassesWithServiceAttributeTestClasses.GenericSelfRegistrationClass<object>);
      var assembly = Substitute.For<FakeAssembly>();
      assembly.ExportedTypes.Returns(new[] { interfaceType, implementationType, selfRegistrationType, selfRegistrationAbstractType, selfRegistrationGenericType, selfRegistrationAbstractGenericType });
      assembly.GetExportedTypes().Returns(x => assembly.ExportedTypes);

      // Act
      serviceCollection.AddClassesWithServiceAttribute(assembly);

      // Assert
      serviceCollection.Should().HaveCount(3);
      serviceCollection.Should().Contain(x => x.ImplementationType == implementationType && x.Lifetime == ServiceLifetime.Singleton);
      serviceCollection.Should().Contain(x => x.ServiceType == interfaceType && x.Lifetime == ServiceLifetime.Singleton);
      serviceCollection.Should().Contain(x => x.ImplementationType == selfRegistrationType && x.Lifetime == ServiceLifetime.Transient);
      serviceCollection.Should().Contain(x => x.ServiceType == selfRegistrationType && x.Lifetime == ServiceLifetime.Transient);
    }

    [Theory]
    [InlineData("*AnotherSimpleTestClass")]
    [InlineData("*Another*")]
    [InlineData("TTT.*Another*")]
    public void AddByWildcard_ValidTypes_ServicesCollectionWithMatchingType(string pattern)
    {
      // Arrange
      IServiceCollection serviceCollection = new ServiceCollection();

      var simpleTestClassType = typeof(AddByWildcardTestClasses.SimpleTestClass);
      var anotherSimpleTestClassType = typeof(AddByWildcardTestClasses.AnotherSimpleTestClass);
      var assembly = Substitute.For<FakeAssembly>();
      assembly.ExportedTypes.Returns(new[] { simpleTestClassType, anotherSimpleTestClassType });
      assembly.GetExportedTypes().Returns(x => assembly.ExportedTypes);

      // Act
      serviceCollection.AddByWildcard(Lifetime.Singleton, pattern, assembly);

      // Assert
      serviceCollection.Should().HaveCount(1);
      serviceCollection.Should().Contain(x => x.ImplementationType == anotherSimpleTestClassType && x.Lifetime == ServiceLifetime.Singleton);
      serviceCollection.Should().Contain(x => x.ServiceType == anotherSimpleTestClassType && x.Lifetime == ServiceLifetime.Singleton);
    }

    [Fact]
    public void AddByWildcard_WhenCurrentAssemblyIsNull_ServicesCollectionWithMatchingType()
    {
      // Arrange
      IServiceCollection serviceCollection = new ServiceCollection();

      var diConfigurator = typeof(DiConfigurator);
      ServiceCollectionExtensions.CallingAssembly = null;

      // Act
      serviceCollection.AddByWildcard(Lifetime.Singleton, "*DiConfigurator*", null);

      // Assert
      serviceCollection.Should().HaveCount(1);
      serviceCollection.Should().Contain(x => x.ServiceType == diConfigurator && x.Lifetime == ServiceLifetime.Singleton);
    }

    [Fact]
    public void Add_WithTypesParam_ReturnsServicesCollectionWithNewTypes()
    {
      // Arrange
      IServiceCollection serviceCollection = new ServiceCollection();
      var implementationType = typeof(AddClassesWithServiceAttributeTestClasses.ImplementationClass);

      // Act
      serviceCollection.Add(Lifetime.Singleton, implementationType);

      // Assert
      serviceCollection.Should().HaveCount(1);
      serviceCollection.Should().Contain(x => x.ImplementationType == implementationType && x.Lifetime == ServiceLifetime.Singleton);
    }

    [Fact]
    public void Add_WithTypeParam_ReturnsServicesCollectionWithNewType()
    {
      // Arrange
      IServiceCollection serviceCollection = new ServiceCollection();

      // Act
      serviceCollection.Add<AddClassesWithServiceAttributeTestClasses.ImplementationClass>(Lifetime.Singleton);

      // Assert
      serviceCollection.Should().HaveCount(1);
      serviceCollection.Should().Contain(x => x.ImplementationType == typeof(AddClassesWithServiceAttributeTestClasses.ImplementationClass) && x.Lifetime == ServiceLifetime.Singleton);
    }

    [Theory]
    [InlineData(Lifetime.Singleton, ServiceLifetime.Singleton)]
    [InlineData(Lifetime.Transient, ServiceLifetime.Transient)]
    public void Add_WithOneType_ReturnsServicesCollectionWithNewType(Lifetime lifetime, ServiceLifetime serviceLifetime)
    {
      // Arrange
      IServiceCollection serviceCollection = new ServiceCollection();
      var implementationType = typeof(AddClassesWithServiceAttributeTestClasses.ImplementationClass);

      // Act
      serviceCollection.Add(implementationType, lifetime);

      // Assert
      serviceCollection.Should().HaveCount(1);
      serviceCollection.Should().Contain(x => x.ImplementationType == implementationType && x.Lifetime == serviceLifetime);
    }

    [Theory]
    [InlineData(Lifetime.Singleton, ServiceLifetime.Singleton)]
    [InlineData(Lifetime.Transient, ServiceLifetime.Transient)]
    public void Add_WithTypeAndImplementation_ReturnsServicesCollectionWithNewType(Lifetime lifetime, ServiceLifetime serviceLifetime)
    {
      // Arrange
      IServiceCollection serviceCollection = new ServiceCollection();
      var implementationType = typeof(AddClassesWithServiceAttributeTestClasses.ImplementationClass);
      var interfaceType = typeof(AddClassesWithServiceAttributeTestClasses.InterfaceClass);

      // Act
      serviceCollection.Add(interfaceType, implementationType, lifetime);

      // Assert
      serviceCollection.Should().HaveCount(1);
      serviceCollection.Should().Contain(x => x.ImplementationType == implementationType && x.Lifetime == serviceLifetime);
    }

    [Fact]
    public void AddTypesImplementing_WithAssemblies_ReturnsServicesCollectionWithNewType()
    {
      // Arrange
      IServiceCollection serviceCollection = new ServiceCollection();
      var implementationType = typeof(AddClassesWithServiceAttributeTestClasses.ImplementationClass);
      var interfaceType = typeof(AddClassesWithServiceAttributeTestClasses.InterfaceClass);
      var assembly = Substitute.For<FakeAssembly>();
      assembly.ExportedTypes.Returns(new[] { interfaceType, implementationType });
      assembly.GetExportedTypes().Returns(x => assembly.ExportedTypes);

      // Act
      serviceCollection.AddTypesImplementing<AddClassesWithServiceAttributeTestClasses.InterfaceClass>(Lifetime.Singleton, assembly);

      // Assert
      serviceCollection.Should().HaveCount(2);
      serviceCollection.Should().Contain(x => x.ImplementationType == implementationType && x.Lifetime == ServiceLifetime.Singleton);
    }

    [Fact]
    public void AddTypesImplementing_WithNullAssemblies_ReturnsServicesCollectionWithNewType()
    {
      // Arrange
      IServiceCollection serviceCollection = new ServiceCollection();

      // Act
      serviceCollection.AddTypesImplementing<AddClassesWithServiceAttributeTestClasses.InterfaceClass>(Lifetime.Singleton, (Assembly[])null);

      // Assert
      serviceCollection.Should().HaveCount(0);
    }

    [Fact]
    public void AddTypesImplementing_WithAssembliesNames_ReturnsServicesCollectionWithNewType()
    {
      // Arrange
      IServiceCollection serviceCollection = new ServiceCollection();
      var implementationType = typeof(AddClassesWithServiceAttributeTestClasses.ImplementationClass);
      var assembly = Substitute.For<FakeAssembly>();
      assembly.ExportedTypes.Returns(new[] { implementationType });
      assembly.GetExportedTypes().Returns(x => assembly.ExportedTypes);
      assembly.GetName().Returns(new AssemblyName { Name = "FakeAssembly" });
      ServiceCollectionExtensions.CurrentDomainAssemblies = new Assembly[] { assembly };

      // Act
      serviceCollection.AddTypesImplementing<AddClassesWithServiceAttributeTestClasses.InterfaceClass>(Lifetime.Singleton, "Fake*");

      // Assert
      serviceCollection.Should().HaveCount(1);
      serviceCollection.Should().Contain(x => x.ImplementationType == implementationType && x.Lifetime == ServiceLifetime.Singleton);
    }

    [Fact]
    public void AddTypesImplementingInCurrentAssembly_WithTypeInTypeParam_ReturnsServicesCollectionWithNewType()
    {
      // Arrange
      IServiceCollection serviceCollection = new ServiceCollection();
      var implementationType = typeof(AddClassesWithServiceAttributeTestClasses.ImplementationClass);
      var assembly = Substitute.For<FakeAssembly>();
      assembly.ExportedTypes.Returns(new[] { implementationType });
      assembly.GetExportedTypes().Returns(x => assembly.ExportedTypes);
      ServiceCollectionExtensions.CallingAssembly = assembly;

      // Act
      serviceCollection.AddTypesImplementingInCurrentAssembly<AddClassesWithServiceAttributeTestClasses.InterfaceClass>(Lifetime.Singleton);

      // Assert
      serviceCollection.Should().HaveCount(1);
      serviceCollection.Should().Contain(x => x.ImplementationType == implementationType && x.Lifetime == ServiceLifetime.Singleton);
    }

    [Fact]
    public void AddControllersInCurrentAssembly_ReturnsServicesCollectionWithControllers()
    {
      // Arrange
      IServiceCollection serviceCollection = new ServiceCollection();
      var testInterface = typeof(AddTestClassesController.TestInterfaceController);
      var controller1 = typeof(AddTestClassesController.TestClass1Controller);
      var controller2 = typeof(AddTestClassesController.TestClass2Controller);
      var assembly = Substitute.For<FakeAssembly>();
      assembly.ExportedTypes.Returns(new[] { testInterface, controller1, controller2 });
      assembly.GetExportedTypes().Returns(x => assembly.ExportedTypes);
      ServiceCollectionExtensions.CallingAssembly = assembly;

      // Act
      serviceCollection.AddControllersInCurrentAssembly<AddTestClassesController.TestInterfaceController>();

      // Assert
      serviceCollection.Should().HaveCount(3);
      serviceCollection.Should().Contain(x => x.ImplementationType == controller1 && x.Lifetime == ServiceLifetime.Transient);
    }

    [Fact]
    public void AddControllers_WithAssemblyFilters_ReturnsServicesCollectionWithControllers()
    {
      // Arrange
      IServiceCollection serviceCollection = new ServiceCollection();
      var testInterface = typeof(AddTestClassesController.TestInterfaceController);
      var controller1 = typeof(AddTestClassesController.TestClass1Controller);
      var assembly = Substitute.For<FakeAssembly>();
      assembly.ExportedTypes.Returns(new[] { testInterface, controller1 });
      assembly.GetExportedTypes().Returns(x => assembly.ExportedTypes);
      assembly.GetName().Returns(new AssemblyName { Name = "FakeAssembly" });
      ServiceCollectionExtensions.CurrentDomainAssemblies = new Assembly[] { assembly };

      // Act
      serviceCollection.AddControllers<AddTestClassesController.TestInterfaceController>("Fake*");

      // Assert
      serviceCollection.Should().HaveCount(2);
      serviceCollection.Should().Contain(x => x.ImplementationType == controller1 && x.Lifetime == ServiceLifetime.Transient);
    }

    [Fact]
    public void AddControllers_WithAssemblyAndClassFilters_ReturnsServicesCollectionWithControllers()
    {
      // Arrange
      IServiceCollection serviceCollection = new ServiceCollection();
      var testInterface = typeof(AddTestClassesController.TestInterfaceController);
      var controller1 = typeof(AddTestClassesController.TestClass1Controller);
      var controller2 = typeof(AddTestClassesController.TestClass2Controller);
      var assembly = Substitute.For<FakeAssembly>();
      assembly.ExportedTypes.Returns(new[] { testInterface, controller1, controller2 });
      assembly.GetExportedTypes().Returns(x => assembly.ExportedTypes);
      assembly.GetName().Returns(new AssemblyName { Name = "FakeAssembly" });
      ServiceCollectionExtensions.CurrentDomainAssemblies = new Assembly[] { assembly };

      // Act
      serviceCollection.AddControllers<AddTestClassesController.TestInterfaceController>(new[] { "Fake*" }, "*TestClass?Controller");

      // Assert
      serviceCollection.Should().HaveCount(2);
      serviceCollection.Should().Contain(x => x.ImplementationType == controller1 && x.Lifetime == ServiceLifetime.Transient);
    }

    [Fact]
    public void AddControllers_WithAssemblyButWithoutClassFilters_ReturnsServicesCollectionWithAllControllers()
    {
      // Arrange
      IServiceCollection serviceCollection = new ServiceCollection();
      var testInterface = typeof(AddTestClassesController.TestInterfaceController);
      var controller1 = typeof(AddTestClassesController.TestClass1Controller);
      var controller2 = typeof(AddTestClassesController.TestClass2Controller);
      var assembly = Substitute.For<FakeAssembly>();
      assembly.ExportedTypes.Returns(new[] { testInterface, controller1, controller2 });
      assembly.GetExportedTypes().Returns(x => assembly.ExportedTypes);
      assembly.GetName().Returns(new AssemblyName { Name = "FakeAssembly" });
      ServiceCollectionExtensions.CurrentDomainAssemblies = new Assembly[] { assembly };

      // Act
      serviceCollection.AddControllers<AddTestClassesController.TestInterfaceController>(new[] { "Fake*" }, null);

      // Assert
      serviceCollection.Should().HaveCount(3);
      serviceCollection.Should().Contain(x => x.ImplementationType == controller1 && x.Lifetime == ServiceLifetime.Transient);
    }

    [Fact]
    public void AddControllers_WithAssemblyAndClassFiltersWhenCurrentDomainAssemblies_ReturnsServicesCollectionWithControllers()
    {
      // Arrange
      IServiceCollection serviceCollection = new ServiceCollection();
      var controller1 = typeof(AddTestClassesController.TestClass1Controller);
      ServiceCollectionExtensions.CurrentDomainAssemblies = null;

      // Act
      serviceCollection.AddControllers<AddTestClassesController.TestInterfaceController>(new [] {"*Foundation*"}, "*Controller*");

      // Assert
      serviceCollection.Should().HaveCount(3);
      serviceCollection.Should().Contain(x => x.ImplementationType == controller1 && x.Lifetime == ServiceLifetime.Transient);
    }

    [Fact]
    public void AddTypesImplementing_WithNotSupportedException_ReturnsEmptyTypes()
    {
      // Arrange
      IServiceCollection serviceCollection = new ServiceCollection();
      var assembly = Substitute.For<FakeAssembly>();
      assembly.GetExportedTypes().Throws(new NotSupportedException());

      // Act
      serviceCollection.AddTypesImplementing<AddClassesWithServiceAttributeTestClasses.InterfaceClass>(Lifetime.Singleton, assembly);

      // Assert
      serviceCollection.Should().HaveCount(0);
    }

    [Fact]
    public void AddTypesImplementing_WithFileLoadException_ReturnsEmptyTypes()
    {
      // Arrange
      IServiceCollection serviceCollection = new ServiceCollection();
      var assembly = Substitute.For<FakeAssembly>();
      assembly.GetExportedTypes().Throws(new FileLoadException());

      // Act
      serviceCollection.AddTypesImplementing<AddClassesWithServiceAttributeTestClasses.InterfaceClass>(Lifetime.Singleton, assembly);

      // Assert
      serviceCollection.Should().HaveCount(0);
    }

    [Fact]
    public void AddTypesImplementing_WithReflectionTypeLoadException_ReturnsEmptyTypes()
    {
      // Arrange
      IServiceCollection serviceCollection = new ServiceCollection();
      var assembly = Substitute.For<FakeAssembly>();
      assembly.GetExportedTypes().Throws(new ReflectionTypeLoadException(new[] { typeof(object) }, new Exception[0]));

      // Act
      serviceCollection.AddTypesImplementing<AddClassesWithServiceAttributeTestClasses.InterfaceClass>(Lifetime.Singleton, assembly);

      // Assert
      serviceCollection.Should().HaveCount(0);
    }

    [Fact]
    public void AddTypesImplementing_WithException_ReturnsEmptyTypes()
    {
      // Arrange
      IServiceCollection serviceCollection = new ServiceCollection();
      var assembly = Substitute.For<FakeAssembly>();
      assembly.GetExportedTypes().Throws(new Exception());

      // Act
      Action act = () => serviceCollection.AddTypesImplementing<AddClassesWithServiceAttributeTestClasses.InterfaceClass>(Lifetime.Singleton, assembly);

      // Assert
      act.Should().Throw<InvalidOperationException>();
    }
  }

  public static class AddClassesWithServiceAttributeTestClasses
  {
    public class InterfaceClass
    {
    }

    [Service(typeof(InterfaceClass), Lifetime = Lifetime.Singleton)]
    public class ImplementationClass : InterfaceClass
    {
    }

    [Service(Lifetime = Lifetime.Transient)]
    public class SelfRegistrationClass
    {
    }

    [Service(Lifetime = Lifetime.Transient)]
    public abstract class AbstractSelfRegistrationClass
    {
    }

    [Service(Lifetime = Lifetime.Transient)]
    public abstract class AbstractGenericSelfRegistrationClass<T>
    {
    }

    [Service(Lifetime = Lifetime.Transient)]
    public class GenericSelfRegistrationClass<T>
    {
    }
  }

  public static class AddTestClassesController
  {
    public class TestInterfaceController
    {
    }

    public class TestClass1Controller : TestInterfaceController
    {
    }

    public class TestClass2Controller : TestInterfaceController
    {
    }
  }

  public static class AddByWildcardTestClasses
  {
    public class SimpleTestClass
    {
    }

    public class AnotherSimpleTestClass
    {
    }
  }
}