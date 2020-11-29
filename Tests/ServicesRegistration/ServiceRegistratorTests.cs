using System;
using System.Reflection;
using NSubstitute;
using NUnit.Framework;
using simple_crud.ServicesRegistration;

namespace Tests.ServicesRegistration
{
    public sealed class ServiceRegistratorTests
    {
        [TestCase(RegisterMethod.Singleton)]
        [TestCase(RegisterMethod.Transient)]
        public void Register_ShouldProcessTypeWithProperAttribute(RegisterMethod registerMethod)
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
            Register(serviceRegistrator, RegisterStub, registerMethod);

            // Assert
            Assert.That(dummyClassRegisteredCount, Is.EqualTo(1));
        }

        [TestCase(RegisterMethod.Singleton)]
        [TestCase(RegisterMethod.Transient)]
        public void Register_ShouldProcessTypeWithProperAttributeForDerivingClass(RegisterMethod registerMethod)
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
            Register(serviceRegistrator, RegisterStub, registerMethod);

            // Assert
            Assert.That(dummyClassRegisteredCount, Is.EqualTo(1));
        }

        [TestCase(RegisterMethod.Singleton)]
        [TestCase(RegisterMethod.Transient)]
        public void Register_ShouldProcessTypeWithProperAttributeForInterfaceImplementingClass(RegisterMethod registerMethod)
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
            Register(serviceRegistrator, RegisterStub, registerMethod);

            // Assert
            Assert.That(dummyClassRegisteredCount, Is.EqualTo(1));
        }

        [TestCase(RegisterMethod.Singleton)]
        [TestCase(RegisterMethod.Transient)]
        public void Register_ShouldThrowWhenClassDoesNotImplementGivenInterface(RegisterMethod registerMethod)
        {
            // Arrange
            var typesProvider = Substitute.For<ITypesProvider>();
            typesProvider.GetTypes().Returns(new[] { typeof(IDummyClassImplementingInterface).GetTypeInfo(), typeof(DummyClassNotImplementingInterface).GetTypeInfo() });
            var serviceRegistrator = new ServiceRegistrator(typesProvider);

            // Act
            // Assert
            Assert.That(() => Register(serviceRegistrator, (toImplement, implementation) => { }, registerMethod),
                Throws.InstanceOf<RegistrationException>());
        }

        [TestCase(RegisterMethod.Singleton)]
        [TestCase(RegisterMethod.Transient)]
        public void Register_ShouldThrowWhenClassDoesNotDeriveFromDeclaredClass(RegisterMethod registerMethod)
        {
            // Arrange
            var typesProvider = Substitute.For<ITypesProvider>();
            typesProvider.GetTypes().Returns(new[] { typeof(DummyClasNotDerivingFromDeclaredBase).GetTypeInfo(), typeof(DummyClass).GetTypeInfo() });
            var serviceRegistrator = new ServiceRegistrator(typesProvider);

            // Act
            // Assert
            Assert.That(() => Register(serviceRegistrator, (toImplement, implementation) => { }, registerMethod),
                Throws.InstanceOf<RegistrationException>());
        }

        [TestCase(RegisterMethod.Singleton)]
        [TestCase(RegisterMethod.Transient)]
        public void Register_ShouldThrowWhenClassDeclaredToImplementDerivingClass(RegisterMethod registerMethod)
        {
            // Arrange
            var typesProvider = Substitute.For<ITypesProvider>();
            typesProvider.GetTypes().Returns(new[] { typeof(DummyClassDeclaringImplementationOfDerivingClass).GetTypeInfo(), typeof(DummyClass4).GetTypeInfo() });
            var serviceRegistrator = new ServiceRegistrator(typesProvider);

            // Act
            // Assert
            Assert.That(() => Register(serviceRegistrator, (toImplement, implementation) => { }, registerMethod),
                Throws.InstanceOf<RegistrationException>());
        }

        [Test]
        public void RegisterTransient_ShouldRegisterOnlyOwnComponents()
        {
            // Arrange
            var typesProvider = Substitute.For<ITypesProvider>();
            typesProvider.GetTypes().Returns(new[] { typeof(TransientNotSingletonClass).GetTypeInfo(), typeof(SingletonNotTransientClass).GetTypeInfo() });
            var serviceRegistrator = new ServiceRegistrator(typesProvider);
            var singletonsCount = 0;
            var transientCount = 0;
            void RegisterStub(Type toImplement, Type implementation)
            {
                if (implementation == typeof(SingletonNotTransientClass) && toImplement == typeof(SingletonNotTransientClass)) singletonsCount++;
                if (implementation == typeof(TransientNotSingletonClass) && toImplement == typeof(TransientNotSingletonClass)) transientCount++;
            }
            
            // Act
            Register(serviceRegistrator, RegisterStub, RegisterMethod.Transient);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(singletonsCount, Is.EqualTo(0));
                Assert.That(transientCount, Is.EqualTo(1));
            });
        }

        [Test]
        public void RegisterSingleton_ShouldRegisterOnlyOwnComponents()
        {
            // Arrange
            var typesProvider = Substitute.For<ITypesProvider>();
            typesProvider.GetTypes().Returns(new[] { typeof(TransientNotSingletonClass).GetTypeInfo(), typeof(SingletonNotTransientClass).GetTypeInfo() });
            var serviceRegistrator = new ServiceRegistrator(typesProvider);
            var singletonsCount = 0;
            var transientCount = 0;
            void RegisterStub(Type toImplement, Type implementation)
            {
                if (implementation == typeof(SingletonNotTransientClass) && toImplement == typeof(SingletonNotTransientClass)) singletonsCount++;
                if (implementation == typeof(TransientNotSingletonClass) && toImplement == typeof(TransientNotSingletonClass)) transientCount++;
            }

            // Act
            Register(serviceRegistrator, RegisterStub, RegisterMethod.Singleton);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(singletonsCount, Is.EqualTo(1));
                Assert.That(transientCount, Is.EqualTo(0));
            });
        }
        public enum RegisterMethod
        {
            Singleton,
            Transient
        }

        private static void Register(ServiceRegistrator registrator, Action<Type, Type> registrationStub, RegisterMethod registerMethod)
        {
            switch (registerMethod)
            {
                case RegisterMethod.Singleton:
                    registrator.RegisterSingletons(registrationStub);
                    return;
                case RegisterMethod.Transient:
                    registrator.RegisterTransient(registrationStub);
                    return;
                default:
                    throw new ArgumentOutOfRangeException("Not supported method!");
            }
        }

        private interface IDummyClassImplementingInterface
        { }

        [Singleton(typeof(DummyClass))]
        [Transient(typeof(DummyClass))]
        private class DummyClass
        { }

        private class DummyClass2 : DummyClass
        { }

        [Singleton(typeof(DummyClass2))]
        [Transient(typeof(DummyClass2))]
        private sealed class DummyClass3 : DummyClass2
        { }

        [Singleton(typeof(IDummyClassImplementingInterface))]
        [Transient(typeof(IDummyClassImplementingInterface))]
        private sealed class DummyClassImplementingInterface : IDummyClassImplementingInterface
        { }

        [Singleton(typeof(IDummyClassImplementingInterface))]
        [Transient(typeof(IDummyClassImplementingInterface))]
        private sealed class DummyClassNotImplementingInterface 
        { }

        [Singleton(typeof(DummyClass))]
        [Transient(typeof(DummyClass))]
        private sealed class DummyClasNotDerivingFromDeclaredBase
        { }

        [Singleton(typeof(DummyClass4))]
        [Transient(typeof(DummyClass4))]
        private class DummyClassDeclaringImplementationOfDerivingClass
        { }

        private sealed class DummyClass4 : DummyClassDeclaringImplementationOfDerivingClass { }

        [Transient(typeof(TransientNotSingletonClass))]
        private sealed class TransientNotSingletonClass
        { }

        [Singleton(typeof(SingletonNotTransientClass))]
        private sealed class SingletonNotTransientClass
        { }
    }
}