#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: ServiceLayerModule.cs
// Date Created: 2014/10/20
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using Autofac;
using DataLayer;
using GenericServices;

namespace ServiceLayer
{
    public class ServiceLayerModule : Module
    {

        /// <summary>
        /// This registers all items in service layer and below
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {

            //Now register the DataLayer
            builder.RegisterModule(new DataLayerModule());

            //Reigister the BizLayer
            //builder.RegisterModule(new BizLayerModule());

            //---------------------------
            //Register service layer: autowire all 
            builder.RegisterAssemblyTypes(GetType().Assembly).AsImplementedInterfaces();

            //and register all the non-generic interfaces interfaces GenericServices assembly
            builder.RegisterAssemblyTypes(typeof(IListService).Assembly).AsImplementedInterfaces();
        }

    }
}
