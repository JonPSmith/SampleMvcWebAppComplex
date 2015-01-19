#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: Test06CustomerAddressDtos.cs
// Date Created: 2014/10/23
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System;
using System.Data.Entity;
using System.Linq;
using DataLayer.GeneratedEf;
using GenericServices;
using GenericServices.Services.Concrete;
using NUnit.Framework;
using ServiceLayer.CustomerServices;
using ServiceLayer.CustomerServices.Support;
using Tests.Helpers;

namespace Tests.UnitTests.Group05ServiceLayer
{
    class Test06CustomerAddressDtos
    {

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            GenericServicesConfig.UseDelegateDecompilerWhereNeeded = true;
        }

        [Test]
        public void Test01AddressListViaServiceOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP
                var service = new DetailService(db);
                var customerWithAddresses = db.Customers.First(x => x.CustomerAddresses.Count > 1);

                //ATTEMPT
                var status = service.GetDetail<CrudCustomerDto>(customerWithAddresses.CustomerID);

                //VERIFY
                status.IsValid.ShouldEqual(true, status.Errors);
                status.Result.CustomerAddresses.ShouldNotEqualNull();
                status.Result.CustomerAddresses.Count().ShouldBeGreaterThan(1);
                status.Result.CustomerAddresses.First().AddressFullAddress.ShouldNotEqualNull();
            }
        }

        [Test]
        public void Test02AddressDetailViaServiceOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP
                var service = new DetailService(db);
                var customerWithAddresses = db.Customers.Include( x => x.CustomerAddresses).First(x => x.CustomerAddresses.Count > 1);

                //ATTEMPT
                var status = service.GetDetail<CrudCustomerAddressDto>(customerWithAddresses.CustomerID, customerWithAddresses.CustomerAddresses.First().AddressID);

                //VERIFY
                status.IsValid.ShouldEqual(true, status.Errors);
                status.Result.Address.ShouldNotEqualNull();
            }
        }

        [Test]
        public void Test05AddressCreateViaServiceOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP
                var setupService = new CreateSetupService(db);
                var service = new CreateService(db);
                var lastCustomer = db.Customers.AsNoTracking().Include(x => x.CustomerAddresses).OrderByDescending(x => x.CustomerID).First();

                //ATTEMPT
                var dto =
                    setupService.GetDto<CrudCustomerAddressDto>()
                        .SetCustomerIdWhenCreatingNewEntry(lastCustomer.CustomerID);
                dto.AddressType = "Unit Test";
                dto.Address.AddressLine1 = "Some street";
                dto.Address.AddressLine2 = Guid.NewGuid().ToString("D");
                dto.Address.City = "some town";
                dto.Address.StateProvince = "a state";
                dto.Address.CountryRegion = "the world";
                dto.Address.PostalCode = "XXX 111";
                var status = service.Create(dto);

                //VERIFY
                status.IsValid.ShouldEqual(true, status.Errors);
                var newLastCustomer = db.Customers.AsNoTracking().Include( x => x.CustomerAddresses.Select( y => y.Address)).OrderByDescending(x => x.CustomerID).First();
                newLastCustomer.CustomerAddresses.Count.ShouldEqual(lastCustomer.CustomerAddresses.Count+1);
                newLastCustomer.CustomerAddresses.OrderByDescending(x => x.AddressID).First().Address.AddressLine2.ShouldEqual(dto.Address.AddressLine2);
            }
        }

        [Test]
        public void Test06AddressUpdateViaServiceOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP
                var setupService = new UpdateSetupService(db);
                var service = new UpdateService(db);
                var lastCustomerWithAddress = db.Customers.Include(x => x.CustomerAddresses).Where( x => x.CustomerAddresses.Count > 0).AsNoTracking().OrderByDescending(x => x.CustomerID).First();

                //ATTEMPT
                var setupStatus = setupService.GetOriginal<CrudCustomerAddressDto>(lastCustomerWithAddress.CustomerID,
                    lastCustomerWithAddress.CustomerAddresses.Last().AddressID);
                setupStatus.IsValid.ShouldEqual(true, setupStatus.Errors);

                setupStatus.Result.Address.AddressLine2 = Guid.NewGuid().ToString("D");
                var status = service.Update(setupStatus.Result);

                //VERIFY
                status.IsValid.ShouldEqual(true, status.Errors);
                var newLastCustomer = db.Customers.AsNoTracking().Include(x => x.CustomerAddresses.Select(y => y.Address)).OrderByDescending(x => x.CustomerID).First();
                newLastCustomer.CustomerAddresses.Count.ShouldEqual(lastCustomerWithAddress.CustomerAddresses.Count);
                newLastCustomer.CustomerAddresses.OrderByDescending(x => x.AddressID).First().Address.AddressLine2.ShouldEqual(setupStatus.Result.Address.AddressLine2);
            }
        }



        [Test]
        public void Test10AddressDeleteNotPossibleViaServiceOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP
                var snap = new CustomerSnapShot(db);
                var service = new DeleteService(db);
                var addressUsedInTwoPlaces =
                    db.Addresses.Include(x => x.CustomerAddresses)
                        .AsNoTracking()
                        .OrderByDescending(x => x.AddressID)
                        .First(x => x.CustomerAddresses.Count > 0 && x.SalesOrderHeaders.Count > 0);

                //ATTEMPT
                var status = service.DeleteWithRelationships<CustomerAddress>(DeleteHelpers.DeleteAssociatedAddress,
                    addressUsedInTwoPlaces.CustomerAddresses.First().CustomerID,
                    addressUsedInTwoPlaces.AddressID);

                //VERIFY
                status.IsValid.ShouldEqual(false, status.Errors);
                status.Errors.Count.ShouldEqual(1);
                status.Errors.First().ErrorMessage.ShouldEqual("This operation failed because another data entry uses this entry.");
                snap.CheckSnapShot(db);
            }
        }

        [Test]
        public void Test11AddressDeleteViaServiceOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP
                var snap = new CustomerSnapShot(db);
                var service = new DeleteService(db);
                var addressUsedInTwoPlaces =
                    db.Addresses.Include(x => x.CustomerAddresses)
                        .AsNoTracking()
                        .OrderByDescending(x => x.AddressID)
                        .First(x => x.CustomerAddresses.Count == 1 && x.SalesOrderHeaders.Count == 0 && x.SalesOrderHeaders1.Count == 0);

                //ATTEMPT
                var status = service.DeleteWithRelationships<CustomerAddress>(DeleteHelpers.DeleteAssociatedAddress,
                    addressUsedInTwoPlaces.CustomerAddresses.First().CustomerID,
                    addressUsedInTwoPlaces.AddressID);

                //VERIFY
                status.IsValid.ShouldEqual(true, status.Errors);
                snap.CheckSnapShot(db, 0, -1, -1);
            }
        }



    }
}
