﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Unity.Builder;
using Unity.Extension;
using Unity.Strategies;

namespace Unity.Specification.Diagnostic.Issues.Container
{
    public abstract partial class SpecificationTests : TestFixtureBase
    {
        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
        }

        #region Test Data

        public class SpyExtension : UnityContainerExtension
        {
            private UnityBuildStage stage;
            private object policy;
            private Type policyType;

            public SpyExtension(BuilderStrategy strategy, UnityBuildStage stage)
            {
                Strategy = strategy;
                this.stage = stage;
            }

            public SpyExtension(BuilderStrategy strategy, UnityBuildStage stage, object policy, Type policyType)
            {
                Strategy = strategy;
                this.stage = stage;
                this.policy = policy;
                this.policyType = policyType;
            }

            protected override void Initialize()
            {
                Context.Strategies.Add(Strategy, this.stage);

                if (this.policy != null)
                {
                    Context.Policies.Set(null, null, this.policyType, this.policy);
                }
            }

            public BuilderStrategy Strategy { get; }
        }

        public class SpyStrategy : BuilderStrategy
        {
            private object existing = null;
            private bool buildUpWasCalled = false;

            public Dictionary<(Type, string), int> BuildUpCallCount { get; private set; } = new Dictionary<(Type, string), int>();              // change

            public override void PreBuildUp(ref BuilderContext context)
            {
                buildUpWasCalled = true;
                existing = context.Existing;
                this.UpdateBuildUpCallCount(context.Type, context.Name);
                UpdateSpyPolicy(ref context);
            }

            public override void PostBuildUp(ref BuilderContext context)
            {
                existing = context.Existing;
            }

            public object Existing
            {
                get { return this.existing; }
            }

            private void UpdateBuildUpCallCount(Type type, string name)                 // change
            {
                var tuple = (type, name);

                if (!this.BuildUpCallCount.ContainsKey(tuple))
                {
                    this.BuildUpCallCount[tuple] = 1;
                    return;
                }

                this.BuildUpCallCount[tuple]++;
            }

            public bool BuildUpWasCalled
            {
                get { return this.buildUpWasCalled; }
            }

            private void UpdateSpyPolicy(ref BuilderContext context)
            {
                SpyPolicy policy = (SpyPolicy)context.Get(null, null, typeof(SpyPolicy));

                if (policy != null)
                {
                    policy.WasSpiedOn = true;
                    policy.Count += 1;
                }
            }
        }

        public class SpyPolicy
        {
            public int Count;

            public bool WasSpiedOn { get; set; }
        }

        public interface ILogger
        {
        }

        public class MockLogger : ILogger
        {
        }

        public class ObjectWithLotsOfDependencies
        {
            private ILogger ctorLogger;
            private ObjectWithOneDependency dep1;
            private ObjectWithTwoConstructorDependencies dep2;
            private ObjectWithTwoProperties dep3;

            public ObjectWithLotsOfDependencies(ILogger logger, ObjectWithOneDependency dep1)
            {
                this.ctorLogger = logger;
                this.dep1 = dep1;
            }

            [Dependency]
            public ObjectWithTwoConstructorDependencies Dep2
            {
                get { return dep2; }
                set { dep2 = value; }
            }

            [InjectionMethod]
            public void InjectMe(ObjectWithTwoProperties dep3)
            {
                this.dep3 = dep3;
            }

            public void Validate()
            {
                Assert.IsNotNull(ctorLogger);
                Assert.IsNotNull(dep1);
                Assert.IsNotNull(dep2);
                Assert.IsNotNull(dep3);

                dep1.Validate();
                dep2.Validate();
                dep3.Validate();
            }

            public ILogger CtorLogger
            {
                get { return ctorLogger; }
            }

            public ObjectWithOneDependency Dep1
            {
                get { return dep1; }
            }

            public ObjectWithTwoProperties Dep3
            {
                get { return dep3; }
            }
        }
        public class ObjectWithOneDependency
        {
            private object inner;

            public ObjectWithOneDependency(object inner)
            {
                this.inner = inner;
            }

            public object InnerObject
            {
                get { return inner; }
            }

            public void Validate()
            {
                Assert.IsNotNull(inner);
            }
        }

        public class ObjectWithTwoProperties
        {
            private object obj1;
            private object obj2;

            [Dependency]
            public object Obj1
            {
                get { return obj1; }
                set { obj1 = value; }
            }

            [Dependency]
            public object Obj2
            {
                get { return obj2; }
                set { obj2 = value; }
            }

            public void Validate()
            {
                Assert.IsNotNull(obj1);
                Assert.IsNotNull(obj2);
                Assert.AreNotSame(obj1, obj2);
            }
        }
        public class ObjectWithTwoConstructorDependencies
        {
            private ObjectWithOneDependency oneDep;

            public ObjectWithTwoConstructorDependencies(ObjectWithOneDependency oneDep)
            {
                this.oneDep = oneDep;
            }

            public ObjectWithOneDependency OneDep
            {
                get { return oneDep; }
            }

            public void Validate()
            {
                Assert.IsNotNull(oneDep);
                oneDep.Validate();
            }
        }

        #endregion
    }
}
