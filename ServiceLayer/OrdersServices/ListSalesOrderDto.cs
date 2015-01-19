#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: ListSalesOrdersDto.cs
// Date Created: 2014/10/24
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DataLayer.GeneratedEf;
using GenericServices.Core;

namespace ServiceLayer.OrdersServices
{   
    /// <summary>
    /// This holds the order status as an enum see http://technet.microsoft.com/en-us/library/ms124879%28v=sql.100%29.aspx for info
    /// </summary>
    public enum SalesOrderHeaderStatuses {  ErrorStatus, InProgress = 1, Approved=2, BackOrder = 3, Rejected = 4, Shipped = 5, Cancelled = 6}
    
    public class ListSalesOrderDto : EfGenericDto<SalesOrderHeader, ListSalesOrderDto>
    {
        [UIHint("HiddenInput")]
        public int SalesOrderID { get; set; }

        public DateTime OrderDate { get; set; }

        public byte Status { get; set; }

        public bool OnlineOrderFlag { get; set; }

        [Required]
        [StringLength(25)]
        public string SalesOrderNumber { get; set; }

        [UIHint("HiddenInput")]
        public int CustomerID { get; set; }

        [DataType(DataType.Currency)]
        public decimal TotalDue { get; set; }

        [Display(Name ="Customer")]
        public string CustomerCompanyName { get; set; }

        /// <summary>
        /// This is used in the Kendo grid to display a string instead of a value
        /// </summary>
        /// <returns></returns>
        public static ICollection<KeyValuePair<int, string>> StatusNameLookup()
        {
            return (from int enumVal in Enum.GetValues(typeof (SalesOrderHeaderStatuses))
                select new KeyValuePair<int, string>(enumVal, Enum.GetName(typeof (SalesOrderHeaderStatuses), enumVal) ?? "Unknown")).ToList();
        }

        //------------------------------------------------------------------
        //protected methods

        protected override CrudFunctions SupportedFunctions
        {
            get { return CrudFunctions.List; }
        }

    }
}
