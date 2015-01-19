using System.Linq;
using DataLayer.GeneratedEf;
using GenericServices;
using GenericServices.Services.Concrete;
using NUnit.Framework;
using ServiceLayer.OrdersServices;
using ServiceLayer.OrdersServices.Support;
using Tests.Helpers;
using System.Data.Entity;

namespace Tests.UnitTests.Group05ServiceLayer
{
    class Test10EditLineItems
    {

        private int _salesOrderId;
        private int _customerId;
        private Product _productToUse;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            GenericServicesConfig.UseDelegateDecompilerWhereNeeded = true;
            
            //we create a SaleOrderHeader to use in these tests
            using (var db = new AdventureWorksLt2012())
            {
                _customerId = db.Customers.OrderBy(x => x.CustomerID).First().CustomerID;
                _productToUse = db.Products.OrderByDescending(x => x.ListPrice).First();
                var dto = new CreateSetupService(db).GetDto<NewOrderDto>();
                dto.CustomerID = _customerId;
                dto.ShipMethod = "Unit Test";
                dto.PurchaseOrderNumber = "Unit Test";
                dto.AccountNumber = "Unit Test";

                var service = new CreateService(db);
                var status = service.Create(dto);
                status.ShouldBeValid();
                _salesOrderId = dto.SalesOrderID;
            }
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            //we delete our unit test SaleOrderHeader
            using (var db = new AdventureWorksLt2012())
            {
                var service = new DeleteService(db);

                var status = service.Delete<SalesOrderHeader>(_salesOrderId);
                status.ShouldBeValid();
            }
        }

        [Test]
        public void Test01CheckSetupWeHaveASalesOrderHeaderOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP
                
                //ATTEMPT

                //VERIFY
                _salesOrderId.ShouldBeGreaterThan(0);
                var soh = db.SalesOrderHeaders.SingleOrDefault(x => x.SalesOrderID == _salesOrderId);
                soh.ShouldNotEqualNull();
                soh.ShipMethod.ShouldEqual("Unit Test");
            }
        }

        [Test]
        public void Test02CheckSetupCustomerIdOk()
        {
            //SETUP

            //ATTEMPT

            //VERIFY
            _customerId.ShouldBeGreaterThan(0);
        }

        [Test]
        public void Test03CheckSetupSalesOrderIsEmptyOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP

                //ATTEMPT

                //VERIFY
                _salesOrderId.ShouldBeGreaterThan(0);
                db.SalesOrderDetails.Count( x => x.SalesOrderID == _salesOrderId).ShouldEqual(0);
                var soh = db.SalesOrderHeaders.Single(x => x.SalesOrderID == _salesOrderId);
                soh.ShouldNotEqualNull();
                soh.TaxAmt.ShouldEqual(0);
                soh.SubTotal.ShouldEqual(0);
                soh.TotalDue.ShouldEqual(0);
            }
        } 

        //----------------------------------------------------------------
        //now try adding/editing line items

        [Test]
        public void Test10AddLineItemCheckHeaderOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP

                //ATTEMPT
                var newOrder = AddLineItem(db, 3);

                //VERIFY
                var soh = db.SalesOrderHeaders.Include( x => x.SalesOrderDetails).SingleOrDefault(x => x.SalesOrderID == _salesOrderId);
                soh.SalesOrderDetails.Count( x => x.SalesOrderDetailID == newOrder.SalesOrderDetailID).ShouldEqual(1);
                CheckTotals(db);
            }
        }

        [Test]
        public void Test11DeleteLineItemCheckHeaderOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP
                AddLineItem(db, 10);
                var newOrder = AddLineItem(db, 5);
                var service = new DeleteService(db);

                //ATTEMPT
                var status = service.DeleteWithRelationships<SalesOrderDetail>(DeleteLineItemHelper.UpdateSalesOrderHeader, newOrder.SalesOrderID, newOrder.SalesOrderDetailID);

                //VERIFY
                status.ShouldBeValid();
                db.SalesOrderDetails.Any( x => x.SalesOrderDetailID == newOrder.SalesOrderDetailID).ShouldEqual(false);
                CheckTotals(db);
            }
        }

        [Test]
        public void Test12EditLineItemCheckHeaderOk()
        {
            using (var db = new AdventureWorksLt2012())
            {
                //SETUP
                var newOrder = AddLineItem(db, 2);
                var detailStatus = new DetailService(db).GetDetail<CrudSalesOrderDetailDto>(newOrder.SalesOrderID, newOrder.SalesOrderDetailID);
                detailStatus.ShouldBeValid();
                var service = new UpdateService(db);

                //ATTEMPT
                detailStatus.Result.UnitPrice = 10000;
                detailStatus.Result.OrderQty = 10;
                var status = service.Update(detailStatus.Result);

                //VERIFY
                status.ShouldBeValid();
                db.SalesOrderDetails.Single(x => x.SalesOrderDetailID == newOrder.SalesOrderDetailID).UnitPrice.ShouldEqual(10000);
                CheckTotals(db);
            }
        }

        //----------------------------------

        private CreateLineItemDto AddLineItem(AdventureWorksLt2012 db, short quantity = 1)
        {
            var service = new CreateService(db);
            var newOrder = new CreateLineItemDto
            {
                SalesOrderID = _salesOrderId,
                CustomerID = _customerId,
                ProductID = _productToUse.ProductID,
                OrderQty = quantity
            };

            //ATTEMPT
            var status = service.Create(newOrder);

            //VERIFY
            status.ShouldBeValid();
            return newOrder;
        }

        private void CheckTotals(AdventureWorksLt2012 db)
        {
            var soh = db.SalesOrderHeaders.Include(x => x.SalesOrderDetails).SingleOrDefault(x => x.SalesOrderID == _salesOrderId);
            soh.SubTotal.ShouldEqual(soh.SalesOrderDetails.Sum( x => x.LineTotal));
            soh.TaxAmt.ShouldEqual(0);
        }
    }
}
