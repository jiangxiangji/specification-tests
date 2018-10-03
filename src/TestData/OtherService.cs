﻿using System;

namespace Unity.Specification.TestData
{
    // A dummy class to support testing type mapping
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
}
