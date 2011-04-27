using System;
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace Unity.Extensions
{
    /// <summary>
    /// Adds <see cref="Lazy{T}"/> and <see cref="IEnumerable{T}"/> support for Unity.
    /// </summary>
    public class LazySupportExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            // Enumerable strategy
            Context.Strategies.AddNew<EnumerableResolutionStrategy>(
                UnityBuildStage.TypeMapping);

            // Lazy policy
            Context.Policies.Set<IBuildPlanPolicy>(
                new LazyResolveBuildPlanPolicy(), typeof(Lazy<>));
        }
    }
}