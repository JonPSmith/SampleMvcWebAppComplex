#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: CrudCustomerAddressDto.cs
// Date Created: 2014/10/23
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using DataLayer.GeneratedEf;
using GenericServices;
using GenericServices.Core;
using ServiceLayer.AddressServices;

namespace ServiceLayer.CustomerServices
{
    public class CrudCustomerAddressDto : EfGenericDto<CustomerAddress, CrudCustomerAddressDto>
    {
        [UIHint("HiddenInput")]
        public int CustomerID { get; set; }

        [UIHint("HiddenInput")]
        public int AddressID { get; set; }

        [Required]
        [StringLength(50)]
        public string AddressType { get; set; }

        public CrudAddressDto Address { get; set; }

        public CrudCustomerAddressDto()
        {
            Address = new CrudAddressDto();
        }

        /// <summary>
        /// This should be used when creating a new entry, as the customerId comes from outside
        /// Can be chained
        /// </summary>
        /// <param name="customerId"></param>
        public CrudCustomerAddressDto SetCustomerIdWhenCreatingNewEntry(int customerId)
        {
            CustomerID = customerId;
            return this;
        }

        //----------------------------------------------------------------------
        //overridden methods from the DTO

        protected override CrudFunctions SupportedFunctions
        {
            get { return CrudFunctions.AllCrudButList | CrudFunctions.DoesNotNeedSetup; }
        }

        protected override Type AssociatedDtoMapping
        {
            get { return typeof(CrudAddressDto); }
        }

        //For update to work we need to include the Address for it to be tracked.
        protected override CustomerAddress FindItemTrackedForUpdate(IGenericServicesDbContext context)
        {
            return context.Set<CustomerAddress>()
                .Where(x => x.CustomerID == CustomerID && x.AddressID == AddressID)
                .Include(x => x.Address).SingleOrDefault();
        }
    }
}
