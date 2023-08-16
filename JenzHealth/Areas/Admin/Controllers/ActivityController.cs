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
using WebApp.Areas.Admin.ViewModels.Report;

namespace WebApp.Areas.Admin.Controllers
{
    public class ActivityController : Controller
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
        IActivityService _activityService;
        public ActivityController()
        {
            _activityService = new ActivityService(new DatabaseEntities(), new LaboratoryService());
        }
        public ActivityController(ActivityService activityService)
        {
            _activityService = activityService;
        }
        #endregion
        // Vendor
        public ActionResult ClothStatus(bool? Saved, bool? found)
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }

            if (Saved == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Saved successfully.";
            }
            return View();
        }

        public JsonResult GetClothesFromAssignTailor(string username)
        {
            var model = _activityService.GetClothesFromAssignTailor(username);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateClothesFromAssignTailor(List<ClothStatusVM> data)
        {
            _activityService.UpdateClothesFromAssignTailor(data);
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        //Take Measurement

        public ActionResult TakeMeasurement(bool? Saved, bool? found)
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            if (Saved == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Saved successfully.";
            }
            if (found == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-danger";
                TempData["AlertMessage"] = "Invalid Bill.";
            }
            return View();

        }
        [HttpPost]
        public ActionResult TakeMeasurement(MeasurementSetupVM vmodel)
        {
            var isExistingBill = db.Billings.Count(x => x.InvoiceNumber == vmodel.BillNumber);
            if (isExistingBill > 0)
            {
                return ComputeMeasurement(vmodel.BillNumber);
            }
            return TakeMeasurement(false, true);
        }
        public ActionResult RequestTracker(bool? Editted)
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
           
            if (Editted == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Saved successfully.";
            }
            return View();
        }
        [HttpPost]
        public ActionResult RequestTracker(RequestTrackerVM vmodel)
        {
            var records = _activityService.TrackRequest(vmodel);

            ViewBag.TableData = records;
            return View(vmodel);
        }
        public ActionResult ComputeMeasurement(string billNumber)
        {
            return View("ComputeMeasurement", _activityService.SetupMeasurementCollection(billNumber));
        }

        public JsonResult UpdateComputedMeausurement(List<MeasurementSetupVM> results)
        {
            _activityService.UpdateComputedMeasurement(results);
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
     
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ClothCollection(RequestTrackerVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _activityService.SaveClothCollector(vmodel);
            }
            return RedirectToAction("RequestTracker", new { Editted = hasSaved });
        }
    }
}