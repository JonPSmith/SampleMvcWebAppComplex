#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: Test02CustomerList.cs
// Date Created: 2014/10/20
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer.GeneratedEf;
using NUnit.Framework;
using ServiceLayer.CustomerServices;
using Tests.Helpers;

namespace Tests.UnitTests.Group02DataLayer
{
    class Test02CustomerList
    {
        [Test]
        public void Test01ListCustomerOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP

                //ATTEMPT
                var customers = db.Customers.ToList();

                //VERIFY
                customers.Count.ShouldBeGreaterThan(0);
            }
        }

        [Test]
        public void Test02ListCustomerWithFullNameOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP

                //ATTEMPT
                var customers = db.Customers.Select(c => c.Title + " " + c.FirstName + " " + c.LastName + " " + c.Suffix).ToList();

                //VERIFY
                customers.Count.ShouldBeGreaterThan(0);
            }
        }

        [Test]
        public void Test04ListCustomerNumOrdersOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP

                //ATTEMPT
                var customerSums =
                    db.Customers.Select(
                        x =>
                            new
                            {
                                x.CustomerID,
                                numOrders =
                                    x.SalesOrderHeaders.Sum(y => (int?) y.SalesOrderDetails.Count())
                            }).ToList();

                //VERIFY
                customerSums.Count.ShouldBeGreaterThan(0);
            }
        }

        [Test]
        public void Test04ListCustomerSumOrdersOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP

                //ATTEMPT
                var customerSums = db.Customers.Select(c => c.SalesOrderHeaders.Sum(x => (decimal?)x.TotalDue)).ToList();

                //VERIFY
                customerSums.Count.ShouldBeGreaterThan(0);
            }
        }

    }
}
