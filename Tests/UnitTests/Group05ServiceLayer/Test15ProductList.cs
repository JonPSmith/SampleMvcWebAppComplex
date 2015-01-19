using System;
using System.Linq;
using DataLayer.GeneratedEf;
using DelegateDecompiler;
using GenericServices;
using GenericServices.Services.Concrete;
using NUnit.Framework;
using ServiceLayer.ProductServices;
using Tests.Helpers;

namespace Tests.UnitTests.Group05ServiceLayer
{
    class Test15ProductList
    {

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            GenericServicesConfig.UseDelegateDecompilerWhereNeeded = true;
        }

        [Test]
        public void Test01ListCustomersCheckIsOnSaleOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP
                
                var service = new ListService(db);

                //ATTEMPT
                var list = service.GetAll<ListProductDto>().Computed().ToList();

                //VERIFY
                list.Count.ShouldBeGreaterThan(0);
                list.Where( c => c.SellStartDate < DateTime.Today && (DateTime.Today <= (c.SellEndDate ?? DateTime.Today))).All( x => x.IsOnSale).ShouldEqual(true);
                list.Where(c => !(c.SellStartDate < DateTime.Today && (DateTime.Today <= (c.SellEndDate ?? DateTime.Today)))).All(x => x.IsOnSale).ShouldEqual(false);
            }
        }

        [Test]
        public void Test05ListCustomersViaServiceProductCategoryIdNonNullOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP
                var service = new ListService(db);

                //ATTEMPT
                var list = service.GetAll<Product>().Computed().ToList();

                //VERIFY
                list.Count.ShouldBeGreaterThan(0);
                list.Any( x => x.ProductCategoryIDNonNull != 0).ShouldEqual(true);
            }
        }

    }
}
