#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: ListCustomerDto.cs
// Date Created: 2014/10/20
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using DataLayer.GeneratedEf;
using GenericServices;
using GenericServices.Core;

namespace ServiceLayer.CustomerServices
{
    public class ListCustomerOrgDto : EfGenericDto<Customer, ListCustomerOrgDto>
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



        //This is the original code for calculating TotalAllOrders. It was replaced by the current ListCustomerDto 
        //which uses a quicker calculation for TotalAllOrders, i.e. SalesOrderHeaders.Any() ? SalesOrderHeaders.Sum(x => x.TotalDue) : 0;

        protected override Action<IMappingExpression<Customer, ListCustomerOrgDto>> AddedDatabaseToDtoMapping
        {
            get
            {
                return m => m.ForMember(d => d.TotalAllOrders,
                    opt => opt.MapFrom(c => c.SalesOrderHeaders.Sum(x => (decimal?)x.TotalDue) ?? 0));
            }
        }

        public override string ToString()
        {
            return string.Format("CustomerID: {0}, FullName: {1}, CompanyName: {2}, TotalOrders: {3}", CustomerID, FullName, CompanyName, TotalAllOrders);
        }
    }
}
