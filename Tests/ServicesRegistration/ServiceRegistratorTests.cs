using System;
using System.Reflection;
using NSubstitute;
using NUnit.Framework;
using simple_crud.ServicesRegistration;

namespace Tests.ServicesRegistration
{
    public sealed class ServiceRegistratorTests
    {
        [Test]
        public void RegisterSingletons_ShouldProcessTypeWithProperAttribute()
        {
            // Arrange
            var dummyClassRegisteredCount = 0;
            var typesProvider = Substitute.For<ITypesProvider>();
            typesProvider.GetTypes().Returns(new[] {typeof(DummyClass).GetTypeInfo()});
            void RegisterStub(Type implementation, Type type)
            {
                if (implementation == type && implementation == typeof(DummyClass)) dummyClassRegisteredCount++;
            }

            var serviceRegistrator = new ServiceRegistrator(typesProvider);

            // Act
            serviceRegistrator.RegisterSingletons(RegisterStub);

            // Assert
            Assert.That(dummyClassRegisteredCount, Is.EqualTo(1));
        }

        [Test]
        public void RegisterSingletons_ShouldProcessTypeWithProperAttributeForDerivingClass()
        {
            // Arrange
            var dummyClassRegisteredCount = 0;
            var typesProvider = Substitute.For<ITypesProvider>();
            typesProvider.GetTypes().Returns(new[] { typeof(DummyClass2).GetTypeInfo(), typeof(DummyClass3).GetTypeInfo() });
            void RegisterStub(Type toImplement, Type implementation)
            {
                if (implementation == typeof(DummyClass3) && toImplement == typeof(DummyClass2)) dummyClassRegisteredCount++;
            }

            var serviceRegistrator = new ServiceRegistrator(typesProvider);

            // Act
            serviceRegistrator.RegisterSingletons(RegisterStub);

            // Assert
            Assert.That(dummyClassRegisteredCount, Is.EqualTo(1));
        }
        
        [Test]
        public void RegisterSingletons_ShouldProcessTypeWithProperAttributeForInterfaceImplementingClass()
        {
            // Arrange
            var dummyClassRegisteredCount = 0;
            var typesProvider = Substitute.For<ITypesProvider>();
            typesProvider.GetTypes().Returns(new[] { typeof(IDummyClassImplementingInterface).GetTypeInfo(), typeof(DummyClassImplementingInterface).GetTypeInfo() });
            void RegisterStub(Type toImplement, Type implementation)
            {
                if (implementation == typeof(DummyClassImplementingInterface) && toImplement == typeof(IDummyClassImplementingInterface)) dummyClassRegisteredCount++;
            }

            var serviceRegistrator = new ServiceRegistrator(typesProvider);

            // Act
            serviceRegistrator.RegisterSingletons(RegisterStub);

            // Assert
            Assert.That(dummyClassRegisteredCount, Is.EqualTo(1));
        }

        [Test]
        public void RegisterSingletons_ShouldThrowWhenClassDoesNotImplementGivenInterface()
        {
            // Arrange
            var typesProvider = Substitute.For<ITypesProvider>();
            typesProvider.GetTypes().Returns(new[] { typeof(IDummyClassImplementingInterface).GetTypeInfo(), typeof(DummyClassNotImplementingInterface).GetTypeInfo() });
            var serviceRegistrator = new ServiceRegistrator(typesProvider);

            // Act
            // Assert
            Assert.That(() => serviceRegistrator.RegisterSingletons((_, _2) => { }),
                Throws.InstanceOf<RegistrationException>());
        }

        [Test]
        public void RegisterSingletons_ShouldThrowWhenClassDoesNotDeriveFromDeclaredClass()
        {
            // Arrange
            var typesProvider = Substitute.For<ITypesProvider>();
            typesProvider.GetTypes().Returns(new[] { typeof(DummyClasNotDerivingFromDeclaredBase).GetTypeInfo(), typeof(DummyClass).GetTypeInfo() });
            var serviceRegistrator = new ServiceRegistrator(typesProvider);

            // Act
            // Assert
            Assert.That(() => serviceRegistrator.RegisterSingletons((_, _2) => { }),
                Throws.InstanceOf<RegistrationException>());
        }

        [Test]
        public void RegisterSingletons_ShouldThrowWhenClassDeclaredToImplementDerivingClass()
        {
            // Arrange
            var typesProvider = Substitute.For<ITypesProvider>();
            typesProvider.GetTypes().Returns(new[] { typeof(DummyClassDeclaringImplementationOfDerivingClass).GetTypeInfo(), typeof(DummyClass4).GetTypeInfo() });
            var serviceRegistrator = new ServiceRegistrator(typesProvider);

            // Act
            // Assert
            Assert.That(() => serviceRegistrator.RegisterSingletons((_, _2) => { }),
                Throws.InstanceOf<RegistrationException>());
        }

        private interface IDummyClassImplementingInterface
        { }

        [Singleton(typeof(DummyClass))]
        private class DummyClass
        { }

        private class DummyClass2 : DummyClass
        { }

        [Singleton(typeof(DummyClass2))]
        private sealed class DummyClass3 : DummyClass2
        { }

        [Singleton(typeof(IDummyClassImplementingInterface))]
        private sealed class DummyClassImplementingInterface : IDummyClassImplementingInterface
        { }

        [Singleton(typeof(IDummyClassImplementingInterface))]
        private sealed class DummyClassNotImplementingInterface 
        { }

        [Singleton(typeof(DummyClass))]
        private sealed class DummyClasNotDerivingFromDeclaredBase
        { }

        [Singleton(typeof(DummyClass4))]
        private class DummyClassDeclaringImplementationOfDerivingClass
        { }

        private sealed class DummyClass4 : DummyClassDeclaringImplementationOfDerivingClass { }
    }
}