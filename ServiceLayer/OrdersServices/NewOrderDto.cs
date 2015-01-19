using System;
using System.ComponentModel.DataAnnotations;
using DataLayer.GeneratedEf;
using GenericLibsBase;
using GenericServices;
using GenericServices.Core;

namespace ServiceLayer.OrdersServices
{
    public class NewOrderDto : EfGenericDto<SalesOrderHeader, NewOrderDto>
    {

        [UIHint("HiddenInput")]
        public int SalesOrderID { get; set; }

        [UIHint("HiddenInput")]
        public int CustomerID { get; set; }

        [StringLength(25)]
        public string PurchaseOrderNumber { get; set; }

        [StringLength(15)]
        public string AccountNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Required]
        [StringLength(50)]
        public string ShipMethod { get; set; }

        protected override CrudFunctions SupportedFunctions
        {
            get { return CrudFunctions.Create; }
        }

        protected override void SetupSecondaryData(IGenericServicesDbContext db, NewOrderDto dto)
        {
            dto.DueDate = DateTime.Today.AddDays(30);
        }

        protected override ISuccessOrErrors<SalesOrderHeader> CreateDataFromDto(IGenericServicesDbContext context, NewOrderDto source)
        {
            var status = base.CreateDataFromDto(context, source);
            if (!status.IsValid) return status;

            //Now we have to fill in various other parts
            status.Result.RevisionNumber = 1;
            status.Result.OrderDate = DateTime.Today;
            status.Result.Status = (byte) SalesOrderHeaderStatuses.InProgress;
            status.Result.SalesOrderNumber = DateTime.Today.ToShortDateString();

            return status;
        }
    }
}
