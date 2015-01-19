#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: DependencyInjectionConfig.cs
// Date Created: 2014/10/20
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using SampleMvcWebAppComplex.Infrastructure;
using ServiceLayer;

namespace SampleMvcWebAppComplex
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterDependencyInjection()
        {
            //This sets up the Autofac container for all levels in the program
            var container = SetupDependencies();

            // Set the dependency resolver for MVC.
            var mvcResolver = new AutofacDependencyResolver(container);
            DependencyResolver.SetResolver(mvcResolver);

            //This allows interfaces etc to be provided as parameters to action methods
            ModelBinders.Binders.DefaultBinder = new DiModelBinder();
        }

        private static IContainer SetupDependencies()
        {

            var builder = new ContainerBuilder();
            Load(builder);

            return builder.Build();
        }

        private static void Load(ContainerBuilder builder)
        {
            //register the service layer, which then registers all other dependencies in the rest of the system
            builder.RegisterModule(new ServiceLayerModule());
        }

    }
}