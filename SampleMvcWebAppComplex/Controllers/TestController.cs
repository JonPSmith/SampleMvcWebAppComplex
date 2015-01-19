#region licence
// =====================================================
// Example code containing some useful methods that will be pulled out into libraries
// Filename: DebugController.cs
// Date Created: 2014/10/21
// © Copyright Selective Analytics 2014. All rights reserved
// =====================================================
#endregion

using System;
using System.Net;
using System.Web.Mvc;
using DataLayer.GeneratedEf;
using GenericLibsBase.Core;
using GenericServices;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SampleMvcWebAppComplex.Infrastructure;
using SampleMvcWebAppComplex.Models;

namespace SampleMvcWebAppComplex.Controllers
{
    public class TestController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NotifySuccess()
        {
            TempData["message"] = new MvcHtmlString("This was a <strong>success</strong> message that will disappear.");
            return View("Index");
        }

        public ActionResult NotifySingleError()
        {
            TempData["errorMessage"] = "This was an error message that should stay up.";
            return View("Index");
        }

        public ActionResult NotifyMultipleErrors()
        {
            TempData["errorMessage"] = "Message line1\nMessage line 2\nMessage line3.";
            return View("Index");
        }

        public ActionResult ThrowException()
        {
            throw new Exception("This is a test exception");
        }

        //--------------------------------------------------------------
        //ajax error tests

        public ActionResult TestAjaxForm()
        {
            return View(new TestAjaxFormModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjaxFormReturn(TestAjaxFormModel model)
        {
            if (!ModelState.IsValid)
            {
                //model errors so return errors
                return ModelState.ReturnModelErrorsAsJson();
            }

            if (!model.ShouldFail)
            {
                return Json(new { SuccessMessage = "This was successful" });
            }

            //else errors, so send back the errors
            var status = new SuccessOrErrors();
            status.AddSingleError("The ShouldFail flag was set, which causes a service failure.");
            status.AddNamedParameterError("ShouldFail", "This should be false for this to work.");
            return status.ReturnErrorsAsJson(model);
        }


        //--------------------------------------------------------------
        //grid test

        public ActionResult TestGridErrors()
        {
            return View();
        }

        public ActionResult AjaxProductDescriptionRead([DataSourceRequest]DataSourceRequest request, IListService service)
        {
            return Json(service.GetAll<ProductDescription>().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AjaxGridGeneralError()
        {
            ModelState.AddModelError("", "This is a general error.");
            return ModelState.ReturnModelErrorsAsJson();
        }

        [Authorize(Roles = "RoleThatDoesNotExist")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AjaxPostNotAuthorized()
        {
            return Json(new[] { "Fred", "Bert", "Joe" });
        }

        [HttpPost]
        public JsonResult AjaxPostExceptionTest(int id)
        {
            throw new NotImplementedException("This is a test of POST Ajax Exception handling");
        }

        public ActionResult TestGridErrorAction(int id)
        {
            TempData["message"] = "Successfully called action with id of " + id;
            return View("TestGridErrors");
        }


    }
}