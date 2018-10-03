﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity.Specification.TestData;

namespace Unity.Specification.Resolution.Array
{
    public abstract partial class SpecificationTests
    {
        [TestMethod]
        public void Array()
        {
            // Act
            var array = UnityContainerExtensions.Resolve<IService[]>(Container);

            // Verify
            Assert.AreEqual(3, Service.Instances);
            Assert.IsNotNull(array);
            Assert.AreEqual(3, array.Length);
        }


        [TestMethod]
        public void Array_OfSimpleClass()
        {
            // Act
            UnityContainerExtensions.RegisterType<SimpleClass[]>(Container, "Array");

            // Verify
            Assert.IsNotNull(UnityContainerExtensions.Resolve<SimpleClass[]>(Container, "Array"));
        }

        [TestMethod]
        public void Array_Lazy()
        {
            // Act
            var array = UnityContainerExtensions.Resolve<Lazy<IService>[]>(Container);

            // Verify
            Assert.AreEqual(0, Service.Instances);
            Assert.IsNotNull(array);
            Assert.AreEqual(3, array.Length);
            Assert.IsNotNull(array[0].Value);
            Assert.IsNotNull(array[1].Value);
            Assert.IsNotNull(array[2].Value);
            Assert.AreEqual(3, Service.Instances);
        }

        [TestMethod]
        public void Array_Func()
        {
            // Act
            var array = UnityContainerExtensions.Resolve<Func<IService>[]>(Container);

            // Verify
            Assert.IsNotNull(array);
            Assert.AreEqual(3, array.Length);
        }

        [TestMethod]
        public void Array_LazyFunc()
        {
            // Act
            var array = UnityContainerExtensions.Resolve<Lazy<Func<IService>>[]>(Container);

            // Verify
            Assert.AreEqual(0, Service.Instances);
            Assert.IsNotNull(array);
            Assert.AreEqual(3, array.Length);
            Assert.IsNotNull(array[0].Value);
            Assert.IsNotNull(array[1].Value);
            Assert.IsNotNull(array[2].Value);
            Assert.AreEqual(0, Service.Instances);
            Assert.IsNotNull(array[0].Value());
            Assert.IsNotNull(array[1].Value());
            Assert.IsNotNull(array[2].Value());
            Assert.AreEqual(3, Service.Instances);
        }

        [TestMethod]
        public void Array_FuncLazy()
        {
            // Act
            var array = UnityContainerExtensions.Resolve<Func<Lazy<IService>>[]>(Container);

            // Verify
            Assert.AreEqual(0, Service.Instances);
            Assert.IsNotNull(array);
            Assert.AreEqual(3, array.Length);
            Assert.IsNotNull(array[0]);
            Assert.IsNotNull(array[1]);
            Assert.IsNotNull(array[2]);
            Assert.AreEqual(0, Service.Instances);
            Assert.IsNotNull(array[0]().Value);
            Assert.IsNotNull(array[1]().Value);
            Assert.IsNotNull(array[2]().Value);
            Assert.AreEqual(3, Service.Instances);
        }


        [TestMethod]
        public void Array_FuncLazyInstance()
        {
            // Setup
            UnityContainerExtensions.RegisterInstance(Container, null, "Instance", new Lazy<IService>(() => new Service()));

            // Act
            var array = UnityContainerExtensions.Resolve<Func<Lazy<IService>>[]>(Container);

            // Verify
            Assert.AreEqual(0, Service.Instances);
            Assert.IsNotNull(array);
            Assert.AreEqual(1, array.Length);
            Assert.IsNotNull(array[0]);
            Assert.AreEqual(0, Service.Instances);
            Assert.IsNotNull(array[0]().Value);
            Assert.AreEqual(1, Service.Instances);
        }

        [TestMethod]
        public void Array_EmptyIfNoObjectsRegistered()
        {
            var results = new List<object>(UnityContainerExtensions.ResolveAll<object>(Container));

            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count);
        }
    }
}
