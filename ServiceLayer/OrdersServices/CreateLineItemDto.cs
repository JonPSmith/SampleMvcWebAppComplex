using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DataLayer.GeneratedEf;
using GenericLibsBase;
using GenericServices;
using GenericServices.Core;

namespace ServiceLayer.OrdersServices
{
    public class CreateLineItemDto : EfGenericDto<SalesOrderDetail, CreateLineItemDto>
    {
        private IGenericServicesDbContext _db;

        [UIHint("HiddenInput")]
        [Range(1, int.MaxValue, ErrorMessage = "The system must define the SalesOrderId.")]
        public int SalesOrderID { get; set; }

        public int SalesOrderDetailID { get; set; }

        [UIHint("HiddenInput")]
        [DoNotCopyBackToDatabase]
        public int CustomerID { get; set; }

        [Display(Name = "Customer")]
        [ReadOnly(true)]
        [DoNotCopyBackToDatabase]
        public string CustomerCompanyName { get; set; }

        [ReadOnly(true)]
        [DoNotCopyBackToDatabase]
        public string SalesOrderNumber { get; set; }

        public short OrderQty { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "You must select a product from the list.")]
        public int ProductID { get; set; }

        protected override CrudFunctions SupportedFunctions
        {
            get { return CrudFunctions.Create; }
        }

        protected override ISuccessOrErrors<SalesOrderDetail> CreateDataFromDto(IGenericServicesDbContext context, CreateLineItemDto source)
        {
            var status = base.CreateDataFromDto(context, source);
            if (!status.IsValid) return status;

            //we read the list price from the products
            status.Result.UnitPrice = context.Set<Product>().Single(x => x.ProductID == source.ProductID).ListPrice;
            status.Result.UnitPriceDiscount = 0;

            return status;
        }

        /// <summary>
        /// We use this simply to capture the dbContext so we can use it in SetupRestOfDto
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dto"></param>
        protected override void SetupSecondaryData(IGenericServicesDbContext db, CreateLineItemDto dto)
        {
            _db = db;
        }

        /// <summary>
        /// This sets up useful information to show the user that he has the right order to update
        /// Note: that the SalesOrderID must be set but the others are purely for display
        /// </summary>
        /// <param name="salesOrderId"></param>
        public void SetupRestOfDto(int salesOrderId)
        {
            SalesOrderID = salesOrderId;
            var salesOrderHeader = _db.Set<SalesOrderHeader>().SingleOrDefault(x => x.SalesOrderID == salesOrderId);
            if (salesOrderHeader == null) return;

            SalesOrderNumber = salesOrderHeader.SalesOrderNumber;
            var customer = _db.Set<Customer>().SingleOrDefault(x => x.CustomerID == salesOrderHeader.CustomerID);
            if (customer == null) return;
            CustomerCompanyName = customer.CompanyName;
        }
    }
}
