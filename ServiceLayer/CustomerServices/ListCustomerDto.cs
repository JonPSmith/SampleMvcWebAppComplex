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
        //As you can see its not that hard, but if you needed to do this for every DTO it gets pertty boring!
        //Note: this version uses the from ... in format as it allows the use of the 'let' statement, 
        //which in this case, produces a better SQL query.

        //protected override IQueryable<ListCustomerDto> ListQueryUntracked(IGenericServicesDbContext context)
        //{
        //    return from x in context.Set<Customer>()
        //        let hasBoughtBefore = x.SalesOrderHeaders.Any()
        //        select new ListCustomerDto
        //        {
        //            CustomerID = x.CustomerID,
        //            CompanyName = x.CompanyName,
        //            FullName = x.Title + (x.Title == null ? "" : " ") + x.FirstName + " " + x.LastName + " " + x.Suffix,
        //            HasBoughtBefore = hasBoughtBefore,
        //            TotalAllOrders = hasBoughtBefore ? x.SalesOrderHeaders.Sum(y => y.TotalDue) : 0
        //        };
        //}

        //If you have read the article https://www.simple-talk.com/dotnet/asp.net/using-entity-framework-with-an-existing-database--user-interface/
        //Then the code below is what I used to use to fill in TotalAllOrders before DelegateDecopiler was updated.
        //Note: later in the same article a look at the resulting SQL produced which caused me to change the calculation. Have a look at Customer.cs in DataLayer

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
