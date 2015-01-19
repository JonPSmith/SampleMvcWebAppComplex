#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: Test08OrderHeaderDtos.cs
// Date Created: 2014/10/24
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System;
using System.Linq;
using DataLayer.GeneratedEf;
using GenericServices;
using GenericServices.Services.Concrete;
using NUnit.Framework;
using ServiceLayer.OrdersServices;
using Tests.Helpers;

namespace Tests.UnitTests.Group05ServiceLayer
{
    class Test08OrderHeaderDtos
    {
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            GenericServicesConfig.UseDelegateDecompilerWhereNeeded = true;
        }

        [Test]
        public void Test01EnumConvertOk()
        {

            //SETUP


            //ATTEMPT
            var lookup = ListSalesOrderDto.StatusNameLookup();

            //VERIFY
            lookup.Count.ShouldEqual(7);

        }

        [Test]
        public void Test02ListSalesOrdersViaServiceOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP
                var service = new ListService(db);

                //ATTEMPT
                var list = service.GetAll<ListSalesOrderDto>().ToList();

                //VERIFY
                list.Count.ShouldBeGreaterThan(0);
                list[0].CustomerCompanyName.ShouldNotEqualNull();
            }
        }

        [Test]
        public void Test05DetailSalesOrderViaServiceOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP
                var service = new DetailService(db);
                var firstOrderHeader = db.SalesOrderHeaders.First();

                //ATTEMPT
                var status = service.GetDetail<CrudSalesOrderDto>(firstOrderHeader.SalesOrderID);

                //VERIFY
                status.IsValid.ShouldEqual(true, status.Errors);
                status.Result.SalesOrderDetails.Count().ShouldBeGreaterThan(0);
            }
        }

        [Test]
        public void Test10UpdateSalesOrderViaServiceOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP
                var setupService = new UpdateSetupService(db);
                var service = new UpdateService(db);
                var firstOrderHeader = db.SalesOrderHeaders.AsNoTracking().First();
                var newComment = Guid.NewGuid().ToString("N");

                //ATTEMPT
                var status = setupService.GetOriginal<CrudSalesOrderDto>(firstOrderHeader.SalesOrderID) ;
                status.ShouldBeValid();
                status.Result.Comment = newComment;
                status.Result.Status = 99;                  //should not be copied back
                var updateStatus = service.Update(status.Result);

                //VERIFY
                updateStatus.ShouldBeValid();
                var readBack = db.SalesOrderHeaders.Single(x => x.SalesOrderID == firstOrderHeader.SalesOrderID);
                readBack.Comment.ShouldEqual(newComment);
                readBack.Status.ShouldEqual(firstOrderHeader.Status);
            }
        }

    }
}
