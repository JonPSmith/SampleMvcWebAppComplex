#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: Address.cs
// Date Created: 2014/10/23
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using DataLayer.Helpers;
using DelegateDecompiler;

namespace DataLayer.GeneratedEf
{

    public partial class Address : IModifiedEntity
    {

        [Computed]
        //Note that AddressLine2 can be null, which gives an extra comma
        public string FullAddress 
        {
            get
            {
                return AddressLine1 + ", " + AddressLine2 + (AddressLine2 == null ? "" :  ", ") + City + ", " + StateProvince + ", " + PostalCode + ", " + CountryRegion;
            }
        }
    }
}
