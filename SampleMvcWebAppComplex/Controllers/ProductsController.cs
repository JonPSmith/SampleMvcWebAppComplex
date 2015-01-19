using System.Linq;
using System.Web.Mvc;
using DataLayer.GeneratedEf;
using GenericServices;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SampleMvcWebAppComplex.Models;
using ServiceLayer.ProductServices;
using ServiceLayer.UiClasses;

namespace SampleMvcWebAppComplex.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Product
        public ActionResult Index(IListService service)
        {
            var dataForList = new ProductListSupportDataModel(ProductListFilters.AvailableForSale,
                service.GetAll<ProductCategory>());
            return View(dataForList);
        }

        public JsonResult IndexListRead([DataSourceRequest]DataSourceRequest request, IListService service)
        {
            var result = service.GetAll<ListProductDto>().OrderBy( x => x.ProductID).ToDataSourceResult(request);
            return Json(result);
        }

        //-----------------------------------------------


        public JsonResult GetProducts(string text, IListService service)
        {
            var products =
                service.GetAll<Product>()
                    .Select(x => new KeyTextClass<int> {Key = x.ProductID, Text = x.ProductNumber + " (" + x.ProductCategory.Name + ", " + x.Name + ")"});

            return string.IsNullOrEmpty(text)
                ? Json(products, JsonRequestBehavior.AllowGet)
                : Json(products.Where(p => p.Text.Contains(text)), JsonRequestBehavior.AllowGet);
        }
    }
}