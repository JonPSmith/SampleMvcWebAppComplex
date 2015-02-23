#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: Test05CustomerDtos.cs
// Date Created: 2014/10/20
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer.GeneratedEf;
using GenericServices;
using GenericServices.Services.Concrete;
using NUnit.Framework;
using ServiceLayer.CustomerServices;
using Tests.Helpers;
using System.Data.Entity;

namespace Tests.UnitTests.Group05ServiceLayer
{
    class Test05CustomerDtos
    {

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            GenericServicesConfig.UseDelegateDecompilerWhereNeeded = true;
        }

        [Test]
        public void Test01ListCustomersViaServiceHaveTotalOrderOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP
                var service = new ListService(db);
                var log = new List<string>();
                db.Database.Log = log.Add;

                //ATTEMPT
                var query = service.GetAll<ListCustomerDto>().Where(x => x.HasBoughtBefore);
                var customers = query.ToList();

                //VERIFY
                customers.Count.ShouldBeGreaterThan(0);
                customers[0].FullName.ShouldNotEqualNull();
                customers.All(x => x.TotalAllOrders > 0).ShouldEqual(true);
            }
        }

        [Test]
        public void Test02ListCustomersViaServiceNoTotalOrdersOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP
                var service = new ListService(db);
                var log = new List<string>();
                db.Database.Log = log.Add;

                //ATTEMPT
                var query = service.GetAll<ListCustomerDto>().Where(x => !x.HasBoughtBefore);
                var customers = query.ToList();

                //VERIFY
                customers.Count.ShouldBeGreaterThan(0);
                customers[0].FullName.ShouldNotEqualNull();
                customers.All(x => x.TotalAllOrders == 0).ShouldEqual(true);
            }
        }


        [Test]
        public void Test05ListCustomersPerformanceOrgOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP
                var service = new ListService(db);
                var log = new List<string>();
                db.Database.Log = log.Add;

                //ATTEMPT
                var query = service.GetAll<ListCustomerOrgDto>().Where(x => x.HasBoughtBefore);
                var customers = query.ToList();

                //VERIFY
                customers.Count.ShouldBeGreaterThan(0);
                customers[0].FullName.ShouldNotEqualNull();
                customers.All(x => x.TotalAllOrders > 0).ShouldEqual(true);
            }
        }

        [Test]
        public void Test06ListCustomersPerformanceVer2Ok()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP
                var service = new ListService(db);
                var log = new List<string>();
                db.Database.Log = log.Add;

                //ATTEMPT
                var query = service.GetAll<ListCustomerVer2Dto>().Where(x => x.HasBoughtBefore);
                var customers = query.ToList();

                //VERIFY
                customers.Count.ShouldBeGreaterThan(0);
                customers[0].FullName.ShouldNotEqualNull();
                customers.All(x => x.TotalAllOrders > 0).ShouldEqual(true);
            }
        }



        [Test]
        public void Test10DetailCrudCustomersViaServiceHasSalesOrdersOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP
                var service = new DetailService(db);
                var customerWithOrders = db.Customers.Include(x => x.SalesOrderHeaders).First(x => x.SalesOrderHeaders.Any());
                //db.Database.Log = Console.WriteLine;

                //ATTEMPT
                var status = service.GetDetail<CrudCustomerDto>(customerWithOrders.CustomerID);

                //VERIFY
                status.ShouldBeValid();
                status.Result.HasBoughtBefore.ShouldEqual(true);
            }
        }


        [Test]
        public void Test11DetailCrudCustomersViaServiceAddressOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP
                var service = new DetailService(db);
                var customerWithOrders = db.Customers.First(x => x.CustomerAddresses.Any());

                //ATTEMPT
                var status = service.GetDetail<CrudCustomerDto>(customerWithOrders.CustomerID);

                //VERIFY
                status.ShouldBeValid();
                status.Result.CustomerAddresses.Count().ShouldBeGreaterThan(0);
            }
        }



    }
}
