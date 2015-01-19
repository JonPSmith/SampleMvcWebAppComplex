#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: Test03CustomerDetail.cs
// Date Created: 2014/10/23
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System.Linq;
using DataLayer.GeneratedEf;
using NUnit.Framework;
using Tests.Helpers;

namespace Tests.UnitTests.Group02DataLayer
{
    class Test03CustomerDetail
    {
        [Test]
        public void Test01DetailWithAddressesOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP

                //ATTEMPT
                var customer = db.Customers.Where( x => x.CustomerAddresses.Count == 2).Select(x => new
                {
                    x.CustomerID,
                    addresses = x.CustomerAddresses.Select(y => new
                    {
                        y.AddressType,
                        address =
                            y.Address.AddressLine1 + ", " + y.Address.AddressLine2 + ", " + y.Address.City + ", " + y.Address.StateProvince + ", " + y.Address.PostalCode + ", " + y.Address.CountryRegion
                    })
                }).First();

                //VERIFY
                customer.addresses.Count().ShouldEqual(2);
            }
        }      

    }
}
