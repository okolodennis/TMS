using WebApp.DAL.DataConnection;
using WebApp.DAL.Entity;
using WebApp.Areas.Admin.Interfaces;
using WebApp.Areas.Admin.Services;
using WebApp.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Admin.Components;

namespace WebApp.Areas.Admin.Controllers
{
    public class ApplicationSettingsController : Controller
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


        // Initializing dependency services
        #region Instanciation
        IApplicationSettingsService _settingService;
        public ApplicationSettingsController()
        {
            _settingService = new ApplicationSettingsService(new DatabaseEntities());
        }
        public ApplicationSettingsController(ApplicationSettingsService settingsService)
        {
            _settingService = settingsService;
        }
        DatabaseEntities db = new DatabaseEntities();
        #endregion

        public ActionResult Manage()
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath)){
                throw new UnauthorizedAccessException();
            }
            return View(_settingService.GetApplicationSettings());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(ApplicationSettingsVM Vmodel, HttpPostedFileBase LogoData, HttpPostedFileBase WatermarkData)
        {
            if (ModelState.IsValid)
            {
                bool saveState = _settingService.UpdateApplicationSettings(Vmodel, LogoData, WatermarkData);
                if(saveState == true)
                {
                    ViewBag.ShowAlert = true;
                    TempData["AlertMessage"] = "Application settings updated successfully.";
                    TempData["AlertType"] = "alert-success";
                }
            }
            return View(_settingService.GetApplicationSettings());
        }


        // ClothTypeMeasurement 
        public ActionResult ManageClothTypeMeasurements(bool? Added, bool? Editted)
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            if (Added == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "ClothTypeMeasurement added successfully.";
            }
            if (Editted == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "ClothTypeMeasurement updated successfully.";
            }
            ViewBag.Measurement = new SelectList(db.Measurements.Where(x => x.IsDeleted == false), "Id", "Name");
            ViewBag.ClothType = new SelectList(db.ClothTypes.Where(x => x.IsDeleted == false), "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult ManageClothTypeMeasurements(ClothTypeMeasurementVM vmodel)
        {
            ViewBag.ClothTypeMeasurements = _settingService.GetClothTypeMeasurement();
            ViewBag.Measurement = new SelectList(db.Measurements.Where(x => x.IsDeleted == false), "Id", "Name");
            ViewBag.ClothType = new SelectList(db.ClothTypes.Where(x => x.IsDeleted == false), "Id", "Name");
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateClothTypeMeasurement(ClothTypeMeasurementVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _settingService.CreateClothTypeMeasurement(vmodel);
            }
            return RedirectToAction("ManageClothTypeMeasurements", new { Added = hasSaved });
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditClothTypeMeasurement(ClothTypeMeasurementVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _settingService.EditClothTypeMeasurement(vmodel);
            }
            return RedirectToAction("ManageClothTypeMeasurements", new { Editted = hasSaved });
        }
        public JsonResult GetClothTypeMeasurement(int id)
        {
           // var model = _settingService.GetClothTypeMeasurement(id);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteClothTypeMeasurement(int id)
        {
            var model = _settingService.DeleteClothTypeMeasurement(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetClothTypeMeasurementAutoComplete(string term)
        {
            var response = _settingService.GetClothTypeMeasurementNameAutoComplete(term);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        // ClothType
        public ActionResult ManageClothTypes(bool? Added, bool? Editted)
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            if (Added == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "ClothType added successfully.";
            }
            if (Editted == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "ClothType updated successfully.";
            }
            ViewBag.ClothTypes = _settingService.GetClothTypes();
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateClothType(ClothTypeVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _settingService.CreateClothType(vmodel);
            }
            return RedirectToAction("ManageClothTypes", new { Added = hasSaved });
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditClothType(ClothTypeVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _settingService.EditClothType(vmodel);
            }
            return RedirectToAction("ManageClothTypes", new { Editted = hasSaved });
        }
        public JsonResult GetClothType(int id)
        {
            var model = _settingService.GetClothType(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteClothType(int id)
        {
            var model = _settingService.DeleteClothType(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetClothTypeAutoComplete(string term)
        {
            var response = _settingService.GetClothTypeAutoComplete(term);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        // Measurement
        public ActionResult ManageMeasurements(bool? Added, bool? Editted)
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            if (Added == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Measurement added successfully.";
            }
            if (Editted == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Measurement updated successfully.";
            }
            ViewBag.Measurements = _settingService.GetMeasurement();
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateMeasurement(MeasurementVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _settingService.CreateMeasurement(vmodel);
            }
            return RedirectToAction("ManageMeasurements", new { Added = hasSaved });
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditMeasurement(MeasurementVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _settingService.EditMeasurement(vmodel);
            }
            return RedirectToAction("ManageMeasurements", new { Editted = hasSaved });
        }
        public JsonResult GetMeasurement(int id)
        {
            var model = _settingService.GetMeasurement(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteMeasurement(int id)
        {
            var model = _settingService.DeleteMeasurement(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }


    }
}