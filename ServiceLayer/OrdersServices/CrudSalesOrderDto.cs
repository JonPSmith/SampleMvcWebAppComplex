#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: UpdateSalesOrderDto.cs
// Date Created: 2014/10/24
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using DataLayer.GeneratedEf;
using GenericLibsBase;
using GenericLibsBase.Core;
using GenericServices;
using GenericServices.Core;
using ServiceLayer.CustomerServices;
using ServiceLayer.UiClasses;

namespace ServiceLayer.OrdersServices
{
    public class CrudSalesOrderDto : EfGenericDto<SalesOrderHeader, CrudSalesOrderDto>
    {

        [UIHint("HiddenInput")]
        public int SalesOrderID { get; set; }

        [DoNotCopyBackToDatabase]
        public byte RevisionNumber { get; set; }

        [DataType(DataType.Date)]
        [DoNotCopyBackToDatabase]
        public DateTime OrderDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [DataType(DataType.Date)]
        [DoNotCopyBackToDatabase]
        public DateTime? ShipDate { get; set; }

        [ScaffoldColumn(false)]
        [DoNotCopyBackToDatabase]
        public byte Status { get; set; }

        [Display (Name = "Status")]
        public SalesOrderHeaderStatuses EnumStatus
        {
            get { return (SalesOrderHeaderStatuses) Status; }
            set { Status = (byte)value; }
        }

        [Display(Name = "Web Order")]
        [DoNotCopyBackToDatabase]
        public bool OnlineOrderFlag { get; set; }

        [Required]
        [StringLength(25)]
        public string SalesOrderNumber { get; set; }

        [StringLength(25)]
        public string PurchaseOrderNumber { get; set; }

        [StringLength(15)]
        public string AccountNumber { get; set; }

        [UIHint("HiddenInput")]
        public int CustomerID { get; set; }

        [UIHint("HiddenInput")]
        public int? ShipToAddressID { get; set; }

        [UIHint("HiddenInput")]
        public int? BillToAddressID { get; set; }

        [Required]
        [StringLength(50)]
        public string ShipMethod { get; set; }

        [StringLength(15)]
        public string CreditCardApprovalCode { get; set; }

        [DataType(DataType.Currency)]
        public decimal SubTotal { get; set; }

        [DataType(DataType.Currency)]
        public decimal TaxAmt { get; set; }

        [DataType(DataType.Currency)]
        public decimal Freight { get; set; }

        [DataType(DataType.Currency)]
        [DoNotCopyBackToDatabase]
        public decimal TotalDue { get; set; }

        public string Comment { get; set; }

        [Display(Name = "Customer Name")]
        [DoNotCopyBackToDatabase]
        public string CustomerFullName { get; set; }

        [Display(Name = "Customer Company")]
        [DoNotCopyBackToDatabase]
        public string CustomerCompanyName { get; set; }

        [Display(Name = "Ship To Address")]
        public string Address1FullAddress { get; set; }

        [Display(Name = "Bill To Address")]
        public string AddressFullAddress { get; set; }

        [Display(Name = "Last updated")]
        [DataType(DataType.Date)]
        [DoNotCopyBackToDatabase]
        public DateTime ModifiedDate { get; set; }

        [DoNotCopyBackToDatabase]
        public IEnumerable<CrudSalesOrderDetailDto> SalesOrderDetails { get; set; }

        //-----------------------------------------------

        //now the parts used for dropdown lists
        [Display(Name = "Ship To Address")]
        public DropDownListType ShipToOptions { get; set; }

        [Display(Name = "Bill To Address")]
        public DropDownListType BillToOptions { get; set; }

        public CrudSalesOrderDto() 
        {
            ShipToOptions = new DropDownListType();
            BillToOptions = new DropDownListType();
        }

        //-----------------------------------------------------
        //overridden methods

        protected override CrudFunctions SupportedFunctions
        {
            get { return CrudFunctions.Detail | CrudFunctions.Update ; }
        }

        protected override Type AssociatedDtoMapping
        {
            get { return typeof(CrudSalesOrderDetailDto); }
        }

        //Need to force Decompile as accesses Customer.FullName, which GenericServices does not spot
        public override bool ForceNeedDecompile
        {
            get { return true; }
        }

        protected override ISuccessOrErrors UpdateDataFromDto(IGenericServicesDbContext context, CrudSalesOrderDto source,
         SalesOrderHeader destination)
        {
            var status = SetupRestOfDto(context, source);

            if (status.IsValid)
                //now we copy the items to the right place
                status = base.UpdateDataFromDto(context, source, destination);

            return status;
        }

        //------------------------------------------------------------------------------------------
        //now the setup parts for the dropdown boxes

        /// <summary>
        /// This is called before a create and an update. It is an update if the dto key property is non zero.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dto"></param>
        protected override void SetupSecondaryData(IGenericServicesDbContext db, CrudSalesOrderDto dto)
        {
            var getPossibleAddresses = db.Set<CustomerAddress>().Include( x => x.Address).Where(x => x.CustomerID == dto.CustomerID).ToList();

            dto.ShipToOptions.SetupDropDownListContent(
                getPossibleAddresses.Select(
                        y =>
                            new KeyValuePair<string, string>(ListCustomerAddressDto.FormCustomerAddressFormatted(y),
                                y.AddressID.ToString("D"))),
                "--- choose ship to address ---");
            if (dto.SalesOrderID != 0 && dto.ShipToAddressID != null)
                //there is an entry, so set the selected value to that
                dto.ShipToOptions.SetSelectedValue(((int)dto.ShipToAddressID).ToString("D"));

            dto.BillToOptions.SetupDropDownListContent(
                getPossibleAddresses.Select(
                        y =>
                            new KeyValuePair<string, string>(ListCustomerAddressDto.FormCustomerAddressFormatted(y),
                                y.AddressID.ToString("D"))),
                "--- choose bill to address ---");
            if (dto.SalesOrderID != 0 && dto.BillToAddressID != null)
                //there is an entry, so set the selected value to that
                dto.BillToOptions.SetSelectedValue(((int)dto.BillToAddressID).ToString("D"));
        }



        private static ISuccessOrErrors SetupRestOfDto(IGenericServicesDbContext db, CrudSalesOrderDto dto)
        {
            var status = SuccessOrErrors.Success("OK if no errors set");

            var shipToAddressId = dto.ShipToOptions.SelectedValueAsInt;
            dto.ShipToAddressID = shipToAddressId != null &&
                                  db.Set<CustomerAddress>()
                                      .SingleOrDefault(
                                          x => x.AddressID == shipToAddressId && x.CustomerID == dto.CustomerID) != null
                ? shipToAddressId
                : null;             //could do more error checking here, but we fail safe if any error

            var billToAddressId = dto.BillToOptions.SelectedValueAsInt;
            dto.BillToAddressID = billToAddressId != null &&
                                  db.Set<CustomerAddress>()
                                      .SingleOrDefault(
                                          x => x.AddressID == billToAddressId && x.CustomerID == dto.CustomerID) != null
                ? billToAddressId
                : null;             //could do more error checking here, but we fail safe if any error

            return status;
        }
    }
}
