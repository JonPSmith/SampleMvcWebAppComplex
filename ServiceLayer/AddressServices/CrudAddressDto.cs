#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: CrudAddressDto.cs
// Date Created: 2014/10/23
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System.ComponentModel.DataAnnotations;
using DataLayer.GeneratedEf;
using GenericServices.Core;

namespace ServiceLayer.AddressServices
{
    public class CrudAddressDto : EfGenericDto<Address, CrudAddressDto>
    {
        [UIHint("HiddenInput")]
        public int AddressID { get; set; }

        [Required]
        [StringLength(60)]
        public string AddressLine1 { get; set; }

        [StringLength(60)]
        public string AddressLine2 { get; set; }

        [Required]
        [StringLength(30)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        public string StateProvince { get; set; }

        [Required]
        [StringLength(50)]
        public string CountryRegion { get; set; }

        [Required]
        [StringLength(15)]
        public string PostalCode { get; set; }

        protected override CrudFunctions SupportedFunctions
        {
            get { return CrudFunctions.AllCrudButList; }
        }
    }
}
