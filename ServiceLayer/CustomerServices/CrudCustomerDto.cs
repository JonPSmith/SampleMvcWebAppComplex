#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: CrudCustomerDto.cs
// Date Created: 2014/10/20
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataLayer.GeneratedEf;
using GenericLibsBase;
using GenericServices;
using GenericServices.Core;

namespace ServiceLayer.CustomerServices
{
    public class CrudCustomerDto : EfGenericDto<Customer, CrudCustomerDto>
    {
        [UIHint("HiddenInput")]
        public int CustomerID { get; set; }

        [StringLength(8)]
        public string Title { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(10)]
        [Display( Description = "Used for suffixes to a name, e.g. Jr.")]
        public string Suffix { get; set; }

        [StringLength(128)]
        public string CompanyName { get; set; }

        [StringLength(50)]
        public string EmailAddress { get; set; }

        [StringLength(25)]
        [Display(Description = "Only holds one phone number so put the most important line here.")]
        public string Phone { get; set; }

        /// <summary>
        /// This is used to decide if the customer has any orders
        /// </summary>
        [UIHint("HiddenInput")]
        public bool HasBoughtBefore { get; set; }

        public IEnumerable<ListCustomerAddressDto> CustomerAddresses { get; set; }

        //--------------------------------------------------------------------------

        protected override CrudFunctions SupportedFunctions
        {
            get { return CrudFunctions.AllCrudButList | CrudFunctions.DoesNotNeedSetup; }
        }

        protected override Type AssociatedDtoMapping
        {
            get { return typeof(ListCustomerAddressDto); }
        }

        /// <summary>
        /// This is called on create. We override it to get at the pasword hash/salt
        /// </summary>
        /// <param name="context"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        protected override ISuccessOrErrors<Customer> CreateDataFromDto(IGenericServicesDbContext context, CrudCustomerDto source)
        {
            //We need to initialise the username and password

            var data = base.CreateDataFromDto(context, source);
            data.Result.PasswordHash = "empty";
            data.Result.PasswordSalt = "empty";

            return data;
        }
    }
}
