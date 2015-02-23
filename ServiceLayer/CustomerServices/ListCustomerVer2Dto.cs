#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: ListCustomerDto.cs
// Date Created: 2014/10/20
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System.ComponentModel.DataAnnotations;
using System.Linq;
using DataLayer.GeneratedEf;
using GenericServices;
using GenericServices.Core;

namespace ServiceLayer.CustomerServices
{
    public class ListCustomerVer2Dto : EfGenericDto<Customer, ListCustomerVer2Dto>
    {

        [UIHint("HiddenInput")]
        public int CustomerID { get; set; }

        /// <summary>
        /// This holds the customer name formed from all its parts
        /// </summary>
        public string FullName { get; set; }

        [StringLength(128)]
        public string CompanyName { get; set; }

        /// <summary>
        /// This is true if a 'Customer' has bought anything before
        /// </summary>
        public bool HasBoughtBefore { get; set; }

        /// <summary>
        /// This holds the total value of all orders placed by this customer
        /// </summary>
        [Display(Name = "Total Value")]
        [DataType(DataType.Currency)]
        public decimal TotalAllOrders { get; set; }

        //-------------------------------------------------
        //overridden methods

        protected override CrudFunctions SupportedFunctions
        {
            get { return CrudFunctions.List; }
        }

        //This code is the second attempt at improving the SQL query for Customers
        //It uses the from ... in format of LINQ which allows a let assignment in it.
        //This produces a better SQL query

        protected override IQueryable<ListCustomerVer2Dto> ListQueryUntracked(IGenericServicesDbContext context)
        {
            return from x in context.Set<Customer>()
                   let hasBoughtBefore = x.SalesOrderHeaders.Any()
                   select new ListCustomerVer2Dto
                   {
                       CustomerID = x.CustomerID,
                       CompanyName = x.CompanyName,
                       FullName = x.Title + (x.Title == null ? "" : " ") + x.FirstName + " " + x.LastName + " " + x.Suffix,
                       HasBoughtBefore = hasBoughtBefore,
                       TotalAllOrders = hasBoughtBefore ? x.SalesOrderHeaders.Sum(y => y.TotalDue) : 0
                   };
        }

        public override string ToString()
        {
            return string.Format("CustomerID: {0}, FullName: {1}, CompanyName: {2}, TotalOrders: {3}", CustomerID, FullName, CompanyName, TotalAllOrders);
        }
    }
}
