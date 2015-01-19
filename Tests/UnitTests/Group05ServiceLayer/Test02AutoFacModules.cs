#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: Test02AutoFacModules.cs
// Date Created: 2014/10/20
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using Autofac;
using GenericServices;
using GenericServices.Services.Concrete;
using NUnit.Framework;
using ServiceLayer;
using Tests.Helpers;

namespace Tests.UnitTests.Group05ServiceLayer
{
    [TestFixture]
    public class Test02AutoFacModules
    {



        //-------------------------------------
        //DataLayer


        //---------------------------------------------
        //ServiceLayer, which also resolves DataLayer


        [Test]
        public void Test15SetupServiceLayerDirectGenerics()
        {
            //SETUP
            var builder = new ContainerBuilder();
            builder.RegisterModule(new ServiceLayerModule());
            var container = builder.Build();

            //ATTEMPT & VERIFY
            using (var lifetimeScope = container.BeginLifetimeScope())
            {
                var instance = lifetimeScope.Resolve<IListService>();
                Assert.NotNull(instance);
                (instance is ListService).ShouldEqual(true);
            }
        }

        //[Test]
        //public void Test16UseServiceLayerDirectGenerics()
        //{
        //    //SETUP
        //    var builder = new ContainerBuilder();
        //    builder.RegisterModule(new ServiceLayerModule());
        //    var container = builder.Build();

        //    //ATTEMPT & VERIFY
        //    using (var lifetimeScope = container.BeginLifetimeScope())
        //    {
        //        var service = lifetimeScope.Resolve<IListService>();
        //        var posts = service.GetAll<Customer>().ToList();
        //        posts.Count.ShouldEqual(3);
        //    }
        //}

    }
}
