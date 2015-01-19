#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: ProductCategory.cs
// Date Created: 2014/10/23
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System.Linq;
using DataLayer.Helpers;
using DelegateDecompiler;

namespace DataLayer.GeneratedEf
{

    public partial class ProductCategory : IModifiedEntity
    {
        [Computed]
        public bool MyTest { get { return Name.Any(x => x == 'B'); } }
        //public decimal MyTest { get { return Products.Aggregate(0m, (sum,p) => sum + p.StandardCost); } }
    }
}
