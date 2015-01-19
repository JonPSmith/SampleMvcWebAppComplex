using System.Collections.Generic;
using System.Linq;
using DataLayer.GeneratedEf;
using ServiceLayer.UiClasses;

namespace SampleMvcWebAppComplex.Models
{
    public enum ProductListFilters { AvailableForSale, NotAvailableForSale}

    public class ProductListSupportDataModel
    {

        public ProductListFilters AvalabilityFilter { get; private set; }

        public ICollection<KeyTextClass<int>> CategoriesLookup { get; private set; }

        public ProductListSupportDataModel(ProductListFilters avalabilityFilter, IQueryable<ProductCategory> productCategories)
        {
            AvalabilityFilter = avalabilityFilter;
            CategoriesLookup =
                productCategories.Where( x => x.Products.Any()).Select(x => new KeyTextClass<int>{ Key = x.ProductCategoryID, Text = x.Name}).ToList();
            CategoriesLookup.Add(new KeyTextClass<int>(0, "Not set"));
        }
    }
}