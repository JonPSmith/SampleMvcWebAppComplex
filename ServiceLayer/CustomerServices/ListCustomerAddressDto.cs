#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: ListCustomerAddressDto.cs
// Date Created: 2014/10/23
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System.ComponentModel.DataAnnotations;
using DataLayer.GeneratedEf;
using GenericServices.Core;

namespace ServiceLayer.CustomerServices
{
    public class ListCustomerAddressDto : EfGenericDto<CustomerAddress, ListCustomerAddressDto>
    {
        [UIHint("HiddenInput")]
        public int AddressID { get; set; }

        [Required]
        [StringLength(50)]
        public string AddressType { get; set; }

        public string AddressFullAddress { get; set; }


        internal static string FormCustomerAddressFormatted(CustomerAddress customerAddress)
        {
            return customerAddress.AddressType + ": " + customerAddress.Address.FullAddress;
        }

        protected override CrudFunctions SupportedFunctions
        {
            get { return CrudFunctions.List; }
        }
    }
}
