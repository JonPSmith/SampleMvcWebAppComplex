#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: Test09OrderDetailDtos.cs
// Date Created: 2014/10/24
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System.Linq;
using DataLayer.GeneratedEf;
using GenericServices;
using GenericServices.Services.Concrete;
using NUnit.Framework;
using ServiceLayer.OrdersServices;
using Tests.Helpers;

namespace Tests.UnitTests.Group05ServiceLayer
{
    class Test09OrderDetailDtos
    {
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            GenericServicesConfig.UseDelegateDecompilerWhereNeeded = true;
        }

        [Test]
        public void Test01ListOrderDetailViaServiceOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP
                var service = new ListService(db);

                //ATTEMPT
                var list = service.GetAll<CrudSalesOrderDetailDto>().ToList();

                //VERIFY
                list.Count.ShouldBeGreaterThan(0);
                list.First().ProductName.ShouldNotEqualNull();
            }
        }

        [Test]
        public void Test05UpdateSetupOrderDetailViaServiceOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP
                var service = new UpdateSetupService(db);
                var firstOrderHeader = db.SalesOrderHeaders.First();

                //ATTEMPT
                var status = service.GetOriginal<CrudSalesOrderDto>(firstOrderHeader.SalesOrderID);

                //VERIFY
                status.ShouldBeValid();
                status.Result.ShipToOptions.KeyValueList.Count.ShouldBeGreaterThan(0);

            }
        }


    }
}
