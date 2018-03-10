﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity.Specification.TestData;

namespace Unity.Specification.Injection
{
    public abstract partial class SpecificationTests
    {
        [TestMethod]
        public void Specification_Injection_Parameter_None()
        {
            var service = _container.Resolve(typeof(object), null, null);

            Assert.IsNotNull(service);
            Assert.IsInstanceOfType(service, typeof(object));
        }

        [TestMethod]
        public void Specification_Injection_Parameter_ObjectWithOneDependency()
        {
            var service = _container.Resolve<ObjectWithOneDependency>();

            Assert.IsNotNull(service);
            Assert.IsInstanceOfType(service, typeof(ObjectWithOneDependency));
            Assert.IsNotNull(service.InnerObject);
        }
    }
}
