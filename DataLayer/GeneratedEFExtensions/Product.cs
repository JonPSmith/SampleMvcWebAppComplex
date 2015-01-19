#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: Product.cs
// Date Created: 2014/10/23
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using DataLayer.Helpers;
using DelegateDecompiler;

namespace DataLayer.GeneratedEf
{
    public partial class Product : IModifiedEntity
    {
        /// <summary>
        /// Because Kendo Grids don't like foreign keys which are nullable this is provided
        /// </summary>
        [Computed]
        public int ProductCategoryIDNonNull { get { return ProductCategoryID == null ? 0 : (int)ProductCategoryID; } }
    }
}
