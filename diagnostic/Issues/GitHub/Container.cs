﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Unity.Specification.Diagnostic.Issues.GitHub
{
    public abstract partial class SpecificationTests
    {
        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        // https://github.com/unitycontainer/container/issues/119
        public void Issue_119()
        {
            // Setup
            Container.RegisterType<IInterface, Class1>(nameof(Class1));
            Container.RegisterType<IInterface, Class2>(nameof(Class2));
            Container.RegisterType<IEnumerable<IInterface>, IInterface[]>();
            Container.RegisterType<A>();

            // Act
            var a = Container.Resolve<A>();
        }

        [TestMethod]
        // https://github.com/unitycontainer/container/issues/126
        public void Issue_126()
        {
            // Setup
            Container.RegisterSingleton<MockLogger>();
            Container.Resolve<MockLogger>();
            Container.RegisterType<ILogger, MockLogger>();
            Container.RegisterType<ObjectWithLotsOfDependencies>();

            // Act
            Container.Resolve<MockLogger>();
            Container.Resolve<ObjectWithLotsOfDependencies>();
            Container.Resolve<ILogger>();
            Container.Resolve<ObjectWithLotsOfDependencies>();

            // Validate
            Assert.IsTrue(((SpyStrategy)Container.Configure<SpyExtension>().Strategy).BuildUpCallCount.ContainsKey((typeof(MockLogger), null)));
            var count = ((SpyStrategy)Container.Configure<SpyExtension>().Strategy).BuildUpCallCount[(typeof(MockLogger), null)];
            Assert.AreEqual(1, count);
        }
    }
}
