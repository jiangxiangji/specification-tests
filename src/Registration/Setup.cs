﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unity.Specification.Registration
{
    public abstract partial class SpecificationTests : TestFixtureBase
    {
        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
        }
    }
}
