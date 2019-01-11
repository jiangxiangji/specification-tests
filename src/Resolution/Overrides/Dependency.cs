﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity.Resolution;

namespace Unity.Specification.Resolution.Overrides
{
    public abstract partial class SpecificationTests
    {
        [TestMethod]
        public void OptionalViaDependency()
        {
            // Setup
            IService x = new Service1();
            IService y = new Service2();

            // Act
            var result = Container.Resolve<Foo>(
                    Override.Dependency("Fred",   x),
                    Override.Dependency("George", y));

            // Verify
            Assert.IsNotNull(result);
            Assert.IsNull(result.Fred);
            Assert.IsNull(result.George);
        }

        [TestMethod]
        public void OptionalViaDependencyAsType()
        {
            // Setup
            IService x = new Service1();
            IService y = new Service2();

            // Act
            var result = Container.Resolve<Foo>(
                    Override.Dependency(typeof(IService), "Fred", x),
                    Override.Dependency(typeof(IService), "George", y));

            // Verify
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Fred);
            Assert.IsNotNull(result.George);
        }

        [TestMethod]
        public void OptionalViaDependencyAsGenericType()
        {
            // Setup
            IService x = new Service1();
            IService y = new Service2();

            // Act
            var result = Container.Resolve<Foo>(
                    Override.Dependency<IService>("Fred", x),
                    Override.Dependency<IService>("George", y));

            // Verify
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Fred);
            Assert.IsNotNull(result.George);
        }



        [TestMethod]
        public void DependencyOverrideOccursEverywhereTypeMatches()
        {
            // Setup
            Container
                .RegisterType<ObjectThatDependsOnSimpleObject>(Resolve.Property("OtherTestObject"))
                .RegisterType<SimpleTestObject>(Invoke.Constructor());

            // Act
            var overrideValue = new SimpleTestObject(15); // arbitrary value

            var result = Container.Resolve<ObjectThatDependsOnSimpleObject>(
                new DependencyOverride<SimpleTestObject>(overrideValue));

            // Verify
            Assert.AreSame(overrideValue, result.TestObject);
            Assert.AreSame(overrideValue, result.OtherTestObject);
        }

        [TestMethod]
        public void ParameterOverrideCanResolveOverride()
        {
            // Setup
            Container.RegisterType<IService, Service1>()
                     .RegisterType<IService, Service2>("other");

            // Act
            var result = Container.Resolve<ObjectTakingASomething>(
                Override.Parameter("something", Resolve.Dependency<IService>("other")));

            // Verify
            Assert.IsInstanceOfType(result.MySomething, typeof(Service2));
        }

        [TestMethod]
        public void CanOverridePropertyValueWithNullWithExplicitInjectionParameter()
        {
            // Setup
            Container
                .RegisterType<ObjectTakingASomething>(Invoke.Constructor(),
                                                      Resolve.Property("MySomething"))
                .RegisterType<IService, Service1>()
                .RegisterType<IService, Service2>("other");

            // Act
            var result = Container.Resolve<ObjectTakingASomething>(
                Override.Property(nameof(ObjectTakingASomething.MySomething), Inject.Parameter<IService>(null))
                        .OnType<ObjectTakingASomething>());

            // Verify
            Assert.IsNull(result.MySomething);
        }

        [TestMethod]
        public void CanOverrideDependencyWithExplicitInjectionParameterValue()
        {
            // Setup
            Container
                .RegisterType<Outer>(Invoke.Constructor(typeof(Inner), 10))
                .RegisterType<Inner>(Invoke.Constructor(20, "ignored"));

            // resolves overriding only the parameter for the Bar instance

            // Act
            var instance = Container.Resolve<Outer>(
                Override.Parameter<int>(Inject.Parameter(50)).OnType<Inner>());

            // Verify
            Assert.AreEqual(10, instance.LogLevel);
            Assert.AreEqual(50, instance.Inner.LogLevel);
        }
    }
}
