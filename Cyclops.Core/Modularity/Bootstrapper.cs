using System.Collections.Generic;
using CommonServiceLocator;
using Unity;
using Unity.Lifetime;

namespace Cyclops.Core.Modularity
{
    public static class Bootstrapper
    {
        public static IServiceLocator Run(IEnumerable<IModule> modules, bool verboseLogging)
        {
            var unity = new UnityContainer();
            var locator = new ServiceLocatorImpl(unity);

            ServiceLocator.SetLocatorProvider(() => locator);

            unity.RegisterInstance<IServiceLocator>(locator, new ContainerControlledLifetimeManager());

            IEnumerable<IModule> modulesRaw = GetAllModulesRaw(modules);
            modulesRaw.ForEach(module => module.Initialize(unity));

            locator.GetInstance<ILogger>().VerboseLogging = verboseLogging;

            return locator;
        }

        private static IEnumerable<IModule> GetAllModulesRaw(IEnumerable<IModule> modules)
        {
            var modulesRaw = new List<IModule>();
            GetAllModulesRaw(modules, modulesRaw);
            return modulesRaw;
        }

        private static void GetAllModulesRaw(IEnumerable<IModule> modules, IList<IModule> modulesRaw)
        {
            modules.ForEach(
                module =>
                    {
                        if (!modulesRaw.Contains(module))
                            modulesRaw.Add(module);
                        GetAllModulesRaw(module.Children, modulesRaw);
                    });
        }
    }
}
