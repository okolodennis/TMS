using WebApp.Areas.Admin.Components;
using WebApp.Areas.Admin.Helpers;
using WebApp.Areas.Admin.Interfaces;
using WebApp.Areas.Admin.Services;
using WebApp.Areas.Admin.ViewModels;
using WebApp.DAL.DataConnection;
using WebApp.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    public class CustomerController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            filterContext.ExceptionHandled = true;

            //Log the error!!
            log.Error("Error trying to do something", filterContext.Exception);
            Global.ErrorMessage = filterContext.Exception.ToString();

            var errorType = filterContext.Exception.GetType().Name;

            switch (errorType)
            {
                case "UnauthorizedAccessException":
                    //Redirect or return a view, but not both.
                    filterContext.Result = RedirectToAction("Unauthorized", "Error", new { area = "Admin" });
                    break;
                default:
                    //Redirect or return a view, but not both.
                    filterContext.Result = RedirectToAction("Error", "Error", new { area = "Admin" });
                    break;
            }

        }


        //Initializing dependency services
        #region Instanciation
        DatabaseEntities db = new DatabaseEntities();
        ICustomerService _customerService;
        public CustomerController()
        {
            _customerService = new CustomerService(new DatabaseEntities());
        }
        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }
        #endregion
        // Vendor
        public ActionResult ManageCustomers(bool? Editted, string uID)
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }

            if (Editted == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Customer updated successfully.";
            }
            ViewBag.GenderList = new SelectList(CustomData.GenderList, "Value", "Text");
            ViewBag.Customers = _customerService.GetCustomers();
            return View();
        }
        public ActionResult RegisterCustomers(bool? Added, string uID)
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }

            if (Added == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Customer with unique ID -  " + uID + "   - added successfully.";
            }
           
            ViewBag.GenderList = new SelectList(CustomData.GenderList, "Value", "Text");
            ViewBag.Customers = _customerService.GetCustomers();
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateCustomer(CustomerVM vmodel)
        {
            string customerUniqueID = "";
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                var response = _customerService.CreateCustomer(vmodel);
                customerUniqueID = response.GetType().GetProperty("customerUniqueID").GetValue(response, null).ToString();
                hasSaved = Convert.ToBoolean(response.GetType().GetProperty("hasSaved").GetValue(response, null));
            }

            return RedirectToAction("ManageCustomers", new { Added = hasSaved, uID = customerUniqueID });
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditCustomer(CustomerVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _customerService.EditCustomer(vmodel);
            }
            return RedirectToAction("ManageCustomers", new { Editted = hasSaved });
        }
        public JsonResult GetCustomer(int id)
        {
            var model = _customerService.GetCustomer(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchCustomerWithIDorPhoneNumber(string value)
        {
            var model = _customerService.SearchCustomerWithIDorPhoneNumber(value);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteCustomer(int id)
        {
            var model = _customerService.DeleteCustomer(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCustomerOrPhoneAutoComplete(string term)
        {
            var response = _customerService.GetCustomerOrPhoneAutoComplete(term);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}