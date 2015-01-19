#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: CrudSalesOrderDetailDto.cs
// Date Created: 2014/10/24
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System.ComponentModel.DataAnnotations;
using DataLayer.GeneratedEf;
using GenericServices;
using GenericServices.Core;

namespace ServiceLayer.OrdersServices
{
    public class CrudSalesOrderDetailDto : EfGenericDto<SalesOrderDetail, CrudSalesOrderDetailDto>
    {
        [UIHint("HiddenInput")]
        public int SalesOrderID { get; set; }

        [UIHint("HiddenInput")]
        public int SalesOrderDetailID { get; set; }

        [DoNotCopyBackToDatabase]
        public string ProductName { get; set; }

        public short OrderQty { get; set; }

        [DataType(DataType.Currency)]
        public decimal UnitPrice { get; set; }

        [DataType(DataType.Currency)]
        public decimal UnitPriceDiscount { get; set; }

        [DataType(DataType.Currency)]
        [DoNotCopyBackToDatabase]
        public decimal LineTotal { get; set; }

        //------------------------------------------------------------------
        //protected methods

        protected override CrudFunctions SupportedFunctions
        {
            get { return CrudFunctions.AllCrud | CrudFunctions.DoesNotNeedSetup; }
        }

    }
}
