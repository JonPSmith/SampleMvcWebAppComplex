#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: OrdersController.cs
// Date Created: 2014/10/24
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DataLayer.GeneratedEf;
using GenericServices;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SampleMvcWebAppComplex.Infrastructure;
using ServiceLayer.OrdersServices;
using ServiceLayer.OrdersServices.Support;

namespace SampleMvcWebAppComplex.Controllers
{
    public class OrdersController : Controller
    {

        [DisplayName("Shipped Orders")]
        public ActionResult Index()
        {
            return View(SalesOrderHeaderStatuses.Shipped);
        }

        [DisplayName("Pending Orders")]
        public ActionResult Pending()
        {
            return View("index", SalesOrderHeaderStatuses.InProgress);
        }

        public JsonResult IndexListRead([DataSourceRequest]DataSourceRequest request, IListService service)
        {
            var result = service.GetAll<ListSalesOrderDto>().OrderBy( x => x.SalesOrderID).ToDataSourceResult(request);
            return Json(result);
        }

        public ActionResult Details(int id, IDetailService service)
        {
            return View(service.GetDetail<CrudSalesOrderDto>(id).Result);
        }

        public ActionResult Edit(int id, IUpdateSetupService service)
        {
            return View(service.GetOriginal<CrudSalesOrderDto>(id).Result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CrudSalesOrderDto salesOrder, IUpdateService service)
        {
            if (!ModelState.IsValid)
            {
                //model errors so return immediately
                service.ResetDto(salesOrder);
                return View(salesOrder);
            }

            var response = service.Update(salesOrder);
            if (response.IsValid)
            {
                TempData["message"] = response.SuccessMessage;
                return RedirectToAction("Index");
            }

            //else errors, so copy the errors over to the ModelState and return to view
            response.CopyErrorsToModelState(ModelState, salesOrder);
            return View(salesOrder);
        }

        //----------------------------------------------------

        [DisplayName("New Order")]
        public ActionResult NewOrder(ICreateSetupService service)
        {
            return View(service.GetDto<NewOrderDto>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewOrder(NewOrderDto newOrder, ICreateService service)
        {
            if (!ModelState.IsValid)
            {
                //model errors so return immediately
                service.ResetDto(newOrder);
                return View(newOrder);
            }

            var response = service.Create(newOrder);
            if (response.IsValid)
            {
                //no success message as in a process
                return RedirectToAction("EditLineItems", new { salesOrderId = newOrder.SalesOrderID});
            }

            //else errors, so copy the errors over to the ModelState and return to view
            response.CopyErrorsToModelState(ModelState, newOrder);
            return View(newOrder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSalesOrder(int salesOrderId, IDeleteService service)
        {
            var response = service.Delete<SalesOrderHeader>(salesOrderId);
            if (response.IsValid)
                return Json(new { SuccessMessage = response.SuccessMessage });

            //else errors, so copy the errors over to the ModelState
            response.CopyErrorsToModelState(ModelState);

            return Json(ModelState.ToDataSourceResult());
        }

        //---------------------------------------------------------------------
        //add/edit line items

        public ActionResult EditLineItems(int salesOrderId, ICreateSetupService service)
        {
            var dto = service.GetDto<CreateLineItemDto>();
            dto.SetupRestOfDto( salesOrderId);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AjaxAddLineItem(CreateLineItemDto newOrder, ICreateService service)
        {
            if (!ModelState.IsValid)
            {
                //model errors so return errors
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return ModelState.ReturnModelErrorsAsJson();
            }

            var response = service.Create(newOrder);
            if (response.IsValid)
            {
                return Json(new { SuccessMessage = response.SuccessMessage });
            }

            //else errors, so send back the errors
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return response.ReturnErrorsAsJson(newOrder);
        }

        /// <summary>
        /// reads only the SalesOrderDetail for the given sales order
        /// </summary>
        /// <param name="request"></param>
        /// <param name="salesOrderId"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        public JsonResult ReadLineItems([DataSourceRequest]DataSourceRequest request, int salesOrderId, IListService service)
        {
            var result = service.GetAll<CrudSalesOrderDetailDto>().Where(x => x.SalesOrderID == salesOrderId).OrderBy(x => x.SalesOrderDetailID).ToDataSourceResult(request);
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateLineItem(CrudSalesOrderDetailDto salesOrder, IUpdateService service)
        {
            if (salesOrder != null && ModelState.IsValid)
            {
                var response = service.Update(salesOrder);
                if (response.IsValid)
                    return Json(new { SuccessMessage = response.SuccessMessage });
                //else errors, so copy the errors over to the ModelState
                response.CopyErrorsToModelState(ModelState, salesOrder);
            }
            return Json(ModelState.ToDataSourceResult());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteLineItem(CrudSalesOrderDetailDto salesOrder, IDeleteService service)
        {
            var response = service.DeleteWithRelationships<SalesOrderDetail>(DeleteLineItemHelper.UpdateSalesOrderHeader, 
                salesOrder.SalesOrderID, salesOrder.SalesOrderDetailID);
            if (response.IsValid)
                return Json(new { SuccessMessage = response.SuccessMessage });

            //else errors, so copy the errors over to the ModelState
            response.CopyErrorsToModelState(ModelState);

            return Json(ModelState.ToDataSourceResult());
        }
    }
}