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
using System.Web.Mvc;
using AutoMapper;
using DataLayer.GeneratedEf;
using GenericServices.Core;

namespace Tests.DtoClasses
{
    public class TestListCustomerDto 
    {

        [HiddenInput(DisplayValue = false)]
        public int CustomerID { get; set; }

        /// <summary>
        /// This holds the customer name formed from all its parts
        /// </summary>
        public string FullName { get; set; }

        [StringLength(128)]
        public string CompanyName { get; set; }

        /// <summary>
        /// This holds the total value of all orders placed by this customer
        /// </summary>
        [Display(Name = "Total Value")]
        [DataType(DataType.Currency)]
        public decimal TotalAllOrders { get; set; }

        public override string ToString()
        {
            return string.Format("CustomerID: {0}, FullName: {1}, CompanyName: {2}, TotalOrders: {3}", CustomerID, FullName, CompanyName, TotalAllOrders);
        }
    }
}
