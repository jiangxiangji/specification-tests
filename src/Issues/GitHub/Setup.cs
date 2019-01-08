﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace Unity.Specification.Issues.GitHub
{

    public abstract partial class SpecificationTests : TestFixtureBase
    {
        [TestInitialize]
        public override void Setup()
        {
            base.Setup();

            Container.RegisterInstance(Name);
        }

        #region Test Data

        public interface IGeneric<T>
        {
        }

        public interface IThing
        {
        }

        public class Thing : IThing
        {
            [InjectionConstructor]
            public Thing()
            {
            }

            public Thing(int i)
            {
            }
        }

        public class Gen1 : IGeneric<IThing>
        {
        }

        public class Gen2 : IGeneric<IThing>
        {
        }

        public interface ILogger
        {
        }

        public class MockLogger : ILogger
        {
        }

        public interface IService
        {
        }

        public class Service : IService, IDisposable
        {
            public string ID { get; } = Guid.NewGuid().ToString();

            public static int Instances = 0;

            public Service()
            {
                Interlocked.Increment(ref Instances);
            }

            public bool Disposed = false;

            public void Dispose()
            {
                Disposed = true;
            }
        }

        public interface IOtherService
        {
        }

        public class OtherService : IService, IOtherService, IDisposable
        {
            [InjectionConstructor]
            public OtherService()
            {

            }

            public OtherService(IUnityContainer container)
            {

            }


            public bool Disposed = false;
            public void Dispose()
            {
                Disposed = true;
            }
        }

        public interface IProctRepository
        {
            string Value { get; }
        }
        public class ProctRepository : IProctRepository
        {
            public string Value { get; }

            public ProctRepository(string base_name = "default.sqlite")
            {
                Value = base_name;
            }
        }

        public class ObjectWithThreeProperties
        {
            [Dependency]
            public string Name { get; set; }

            public object Property { get; set; }

            [Dependency]
            public IUnityContainer Container { get; set; }
        }

        #endregion
    }
}