using DataLayer.GeneratedEf;
using DelegateDecompiler;
using Address = DataLayer.GeneratedEf.Address;

namespace Tests.DtoClasses
{
    class TestComputedAddressDto
    {

        public int CustomerID { get; set; }

        public int AddressID { get; set; }

        public string AddressType { get; set; }

        public Address Address { get; set; }

        [Computed]
        public string FullAddress
        {
            get
            {
                return Address.AddressLine1 + ", " + Address.AddressLine2 + ", " + Address.City + ", "
                       + Address.StateProvince + ", " + Address.PostalCode + ", " + Address.CountryRegion;
            }
        }

    }
}
