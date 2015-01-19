#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: CustomerAddressSnap.cs
// Date Created: 2014/10/23
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System.Linq;
using DataLayer.GeneratedEf;

namespace Tests.Helpers
{
    class CustomerSnapShot
    {

        public int NumCustomers { get; private set; }

        public int NumCustomerAddresses { get; private set; }
        
        public int NumAddresses { get; private set; }


        public CustomerSnapShot(AdventureWorksLt2012 db)
        {
            NumCustomers = db.Customers.Count();
            NumCustomerAddresses = db.CustomerAddresses.Count();
            NumAddresses = db.Addresses.Count();
        }

        //creates snapshot setting zero on everything
        public CustomerSnapShot() { }

        public void CheckSnapShot(AdventureWorksLt2012 db, int customers = 0, int custAddressesChange = 0, int addressesChange = 0)
        {
            var newSnap = new CustomerSnapShot(db);

            newSnap.NumCustomers.ShouldEqual(NumCustomers + customers, "customers wrong");
            newSnap.NumCustomerAddresses.ShouldEqual(NumCustomerAddresses + custAddressesChange, "CustomerAddresses wrong");
            newSnap.NumAddresses.ShouldEqual(NumAddresses + addressesChange, "addresses wrong");
        }
    }
}
