﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unity.Specification.Injection.Method
{
    public abstract partial class SpecificationTests : TestFixtureBase
    {
        [TestInitialize]
        public override void Setup()
        {
            Container = GetContainer();
        }
    }
}
