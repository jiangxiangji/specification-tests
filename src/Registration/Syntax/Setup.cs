﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unity.Specification.Registration.Syntax
{
    public abstract partial class SpecificationTests : TestFixtureBase
    {
        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
        }
    }

    #region Test Data

    public interface IService { }

    #endregion
}
