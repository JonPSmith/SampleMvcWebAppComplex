#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: ListCustomerDto.cs
// Date Created: 2014/10/20
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System.ComponentModel.DataAnnotations;
using DataLayer.GeneratedEf;
using GenericServices.Core;

namespace ServiceLayer.CustomerServices
{
    public class ListCustomerDto : EfGenericDto<Customer, ListCustomerDto>
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

        //The code below shows how to override ListQueryUntracked to achive what DelegateDecompiler and AutoMapper do automatically 

        //protected override IQueryable<ListCustomerDto> ListQueryUntracked(IGenericServicesDbContext context)
        //{
        //    return context.Set<Customer>().Select(x => new ListCustomerDto
        //    {
        //        CustomerID = x.CustomerID,
        //        CompanyName = x.CompanyName,
        //        FullName = x.Title + (x.Title == null ? "" : " ") + x.FirstName + " " + x.LastName + " " + x.Suffix,
        //        HasBoughtBefore = x.SalesOrderHeaders.Any(),
        //        TotalAllOrders = x.SalesOrderHeaders.Sum(y => (decimal?) y.TotalDue) ?? 0
        //    });
        //}

        //If you have read the article https://www.simple-talk.com/dotnet/asp.net/using-entity-framework-with-an-existing-database--user-interface/
        //Then the code below is what I used to use to fill in TotalAllOrders before DelegateDecopiler was updated.
        //Note: later in the same article a look at the resulting SQL produced caused me to change the calculation. Have a look at Customer.cs in DataLayer

        //protected override Action<IMappingExpression<Customer, ListCustomerDto>> AddedDatabaseToDtoMapping
        //{
        //    get
        //    {
        //        return m => m.ForMember(d => d.TotalAllOrders,
        //            opt => opt.MapFrom(c => c.SalesOrderHeaders.Sum(x => (decimal?)x.TotalDue) ?? 0));
        //    }
        //}

        public override string ToString()
        {
            return string.Format("CustomerID: {0}, FullName: {1}, CompanyName: {2}, TotalOrders: {3}", CustomerID, FullName, CompanyName, TotalAllOrders);
        }
    }
}
