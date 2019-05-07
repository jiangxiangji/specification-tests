﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Unity.Specification.Diagnostic.Exceptions
{
    public abstract partial class SpecificationTests
    {
        [TestMethod]
        public void MethodErrorLevel1()
        {
            Exception exception = null;

            // Act
            try { Container.Resolve(typeof(ClassWithMethod)); }
            catch (Exception ex) { exception = ex; }

            // Validate
            Assert.IsNotNull(exception.InnerException);
            Assert.AreEqual(4, exception.InnerException.Data.Count);
        }

        [TestMethod]
        public void MethodErrorLevel2()
        {
            Exception exception = null;

            // Act
            try { Container.Resolve(typeof(ClassDependingOnMethod)); }
            catch (Exception ex) { exception = ex; }

            // Validate
            Assert.IsNotNull(exception.InnerException);
            Assert.AreEqual(7, exception.InnerException.Data.Count);
        }

        [TestMethod]
        public void MethodWithOutParam()
        {
            Exception exception = null;

            // Act
            try { Container.Resolve(typeof(ClassWithOutMethod)); }
            catch (Exception ex) { exception = ex; }

            // Validate
            Assert.IsNotNull(exception.InnerException);
            Assert.AreEqual(1, exception.InnerException.Data.Count);
        }

        [TestMethod]
        public void MethodWithRefParam()
        {
            Exception exception = null;

            // Act
            try { Container.Resolve(typeof(ClassWithRefMethod)); }
            catch (Exception ex) { exception = ex; }

            // Validate
            Assert.IsNotNull(exception.InnerException);
            Assert.AreEqual(1, exception.InnerException.Data.Count);
        }
    }
}
