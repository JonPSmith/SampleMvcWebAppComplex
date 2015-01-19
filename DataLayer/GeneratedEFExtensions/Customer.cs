#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: Customer.cs
// Date Created: 2014/10/23
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System.Linq;
using DataLayer.Helpers;
using DelegateDecompiler;

namespace DataLayer.GeneratedEf
{
    
    public partial class Customer : IModifiedEntity
    {
        [Computed]
        public string FullName { get { return Title + (Title == null ? "" : " ") + FirstName + " " + LastName + " " + Suffix; } }

        /// <summary>
        /// This is true if a 'Customer' has bought anything before
        /// </summary>
        [Computed]
        public bool HasBoughtBefore { get { return SalesOrderHeaders.Any(); } }

        /// <summary>
        /// This holds the total value of all orders placed by this customer
        /// </summary>
        [Computed]
        public decimal TotalAllOrders { get { return SalesOrderHeaders.Any() ? SalesOrderHeaders.Sum(x => x.TotalDue) : 0; } }
    }
}
