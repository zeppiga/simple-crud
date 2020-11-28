using System;
using NUnit.Framework;
using simple_crud;

namespace Tests
{
    public sealed class ServiceRegistratorTests
    {
        [SetUp]
        public void Setup()
        { }

        [Test]
        public void RegisterSingletons_ShouldProcessTypeWithProperAttribute()
        {
            // Arrange
            var dummyClassRegisteredCount = 0;
            void RegisterStub(Type implementation, Type type)
            {
                if (implementation == type && implementation == typeof(DummyClass)) dummyClassRegisteredCount++;
            }

            // Act
            ServiceRegistrator.RegisterSingletons(RegisterStub);

            // Assert
            Assert.That(dummyClassRegisteredCount, Is.EqualTo(1));
        }

        [Test]
        public void RegisterSingletons_ShouldProcessTypeWithProperAttributeForDerivingClass()
        {
            // Arrange
            var dummyClassRegisteredCount = 0;
            void RegisterStub(Type toImplement, Type implementation)
            {
                if (implementation == typeof(DummyClass3) && toImplement == typeof(DummyClass2)) dummyClassRegisteredCount++;
            }

            // Act
            ServiceRegistrator.RegisterSingletons(RegisterStub);

            // Assert
            Assert.That(dummyClassRegisteredCount, Is.EqualTo(1));
        }
        
        [Test]
        public void RegisterSingletons_ShouldProcessTypeWithProperAttributeForInterfaceImplementingClass()
        {
            // Arrange
            var dummyClassRegisteredCount = 0;
            void RegisterStub(Type toImplement, Type implementation)
            {
                if (implementation == typeof(IDummyClassImplementingInterface) && toImplement == typeof(DummyClassImplementingInterface)) dummyClassRegisteredCount++;
            }

            // Act
            ServiceRegistrator.RegisterSingletons(RegisterStub);

            // Assert
            Assert.That(dummyClassRegisteredCount, Is.EqualTo(1));
        }

    }

    [Singleton(typeof(DummyClass))]
    internal class DummyClass
    { }

    internal class DummyClass2 : DummyClass
    { }

    [Singleton(typeof(DummyClass2))]
    internal sealed class DummyClass3 : DummyClass2
    {}

    internal interface IDummyClassImplementingInterface
    { }

    [Singleton(typeof(IDummyClassImplementingInterface))]
    internal class DummyClassImplementingInterface : IDummyClassImplementingInterface
    { }
}