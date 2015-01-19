#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: CustomerController.cs
// Date Created: 2014/10/20
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using DataLayer.GeneratedEf;
using GenericServices;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SampleMvcWebAppComplex.Infrastructure;
using ServiceLayer.CustomerServices;
using ServiceLayer.CustomerServices.Support;

namespace SampleMvcWebAppComplex.Controllers
{
    public class CustomersController : Controller
    {
        // GET: Customer
        [DisplayName("Current Customers")]
        public ActionResult Index()
        {
            return View(true);
        }

        [DisplayName("Potential Customers")]
        public ActionResult NotCustomers()
        {
            return View("Index", false);
        }

        public JsonResult IndexListRead([DataSourceRequest]DataSourceRequest request, IListService service)
        {
            var result = service.GetAll<ListCustomerDto>().OrderBy(x => x.CustomerID).ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int id, IDetailService service)
        {
            return View(service.GetDetail<CrudCustomerDto>(id).Result);
        }

        public ActionResult Edit(int id, IUpdateSetupService service)
        {
            return View(service.GetOriginal<CrudCustomerDto>(id).Result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CrudCustomerDto customer, IUpdateService service)
        {
            if (!ModelState.IsValid)
            {
                //model errors so return immediately
                service.ResetDto(customer);
                return View(customer);
            }

            var response = service.Update(customer);
            if (response.IsValid)
            {
                TempData["message"] = response.SuccessMessage;
                return RedirectToAction(customer.HasBoughtBefore ? "Index" : "NotCustomers");
            }

            //else errors, so copy the errors over to the ModelState and return to view
            response.CopyErrorsToModelState(ModelState, customer);
            return View(customer);
        }

        [DisplayName("New Customer")]
        public ActionResult Create(ICreateSetupService service)
        {
            return View(service.GetDto<CrudCustomerDto>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CrudCustomerDto customer, ICreateService service)
        {
            if (!ModelState.IsValid)
                //model errors so return immediately
                return View(customer);

            var response = service.Create(customer);
            if (response.IsValid)
            {
                TempData["message"] = response.SuccessMessage + " You must now assign a password to this customer.";
                return RedirectToAction("NotCustomers");
            }

            //else errors, so copy the errors over to the ModelState and return to view
            response.CopyErrorsToModelState(ModelState, customer);
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomerDelete(int customerId, IDeleteService service)
        {
            var response = service.Delete<Customer>(customerId);
            if (response.IsValid)
                return Json(new {SuccessMessage = response.SuccessMessage});

            //else errors, so copy the errors over to the ModelState
            response.CopyErrorsToModelState(ModelState);

            return Json(ModelState.ToDataSourceResult());
        }

        public JsonResult GetCompanies(string text, IListService service)
        {
            var companies = service.GetAll<ListCustomerDto>();

            return string.IsNullOrEmpty(text)
                ? Json(companies, JsonRequestBehavior.AllowGet)
                : Json(companies.Where(p => p.CompanyName.Contains(text)), JsonRequestBehavior.AllowGet);
        }

        //-------------------------------------------------------------------------------
        //address related actions

        public ActionResult AddAddress(int customerId, ICreateSetupService service)
        {
            var dto = service.GetDto<CrudCustomerAddressDto>().SetCustomerIdWhenCreatingNewEntry( customerId);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAddress(CrudCustomerAddressDto customerAddress, ICreateService service)
        {
            if (!ModelState.IsValid)
                //model errors so return immediately
                return View(customerAddress);

            var response = service.Create(customerAddress);
            if (response.IsValid)
            {
                TempData["message"] = response.SuccessMessage;
                return RedirectToAction("Details", new { id = customerAddress.CustomerID });
            }

            //else errors, so copy the errors over to the ModelState and return to view
            response.CopyErrorsToModelState(ModelState, customerAddress);
            return View(customerAddress);
        }

        public ActionResult EditAddress(int customerId, int addressId, IUpdateSetupService service)
        {
            return View(service.GetOriginal<CrudCustomerAddressDto>(customerId, addressId).Result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAddress(CrudCustomerAddressDto customerAddress, IUpdateService service)
        {
            if (!ModelState.IsValid)
            {
                //model errors so return immediately
                service.ResetDto(customerAddress);
                return View(customerAddress);
            }

            var response = service.Update(customerAddress);
            if (response.IsValid)
            {
                TempData["message"] = response.SuccessMessage;
                return RedirectToAction("Details", new { id = customerAddress.CustomerID });
            }

            //else errors, so copy the errors over to the ModelState and return to view
            response.CopyErrorsToModelState(ModelState, customerAddress);
            return View(customerAddress);
        }

        public ActionResult DeleteAddress(int customerId, int addressId, IDeleteService service)
        {
            var response = service.DeleteWithRelationships<CustomerAddress>( DeleteHelpers.DeleteAssociatedAddress, customerId, addressId);
            if (response.IsValid)
                TempData["message"] = response.SuccessMessage;
            else
                //else errors, so send back an error message
                TempData["errorMessage"] = new MvcHtmlString(response.ErrorsAsHtml());

            return RedirectToAction("Details", new { id = customerId });
        }

    }
}