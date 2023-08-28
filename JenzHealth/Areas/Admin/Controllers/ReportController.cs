using WebApp.Areas.Admin.Components;
using WebApp.Areas.Admin.Interfaces;
using WebApp.Areas.Admin.Services;
using WebApp.Areas.Admin.ViewModels;
using WebApp.DAL.DataConnection;
using WebApp.DAL.Entity;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using PowerfulExtensions.Linq;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Admin.ViewModels.Report;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebApp.Areas.Admin.Controllers
{
    public class ReportController : Controller
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
        IApplicationSettingsService _settingService;
        IPaymentService _paymentService;
        ISeedService _seedService;
        IUserService _userService;
        ILaboratoryService _laboratoryService;
        IReportService _reportService;
        IActivityService _activityService;
        public ReportController()
        {
            _customerService = new CustomerService(db);
            _laboratoryService = new LaboratoryService(db);
            _paymentService = new PaymentService(db, new UserService());
            _seedService = new SeedService(db);
            _settingService = new ApplicationSettingsService(db);
            _userService = new UserService(db);
            _reportService = new ReportService(db, new PaymentService(), new LaboratoryService(), new SeedService());
        }
        public ReportController(
            CustomerService customerService,
            LaboratoryService laboratoryService,
            PaymentService paymentService,
            SeedService seedService,
            ApplicationSettingsService settingService,
            UserService userService,
            ReportService reportService
            )
        {
            _customerService = customerService;
            _laboratoryService = laboratoryService;
            _paymentService = paymentService;
            _seedService = seedService;
            _settingService = settingService;
            _userService = userService;
            _reportService = reportService;
        }
        #endregion

      

        public ActionResult ClothCollectors()
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            return View();
        }
        public ActionResult CustomerReport()
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            var model = new CustomerReportVM();
            return View(model);
        }
        [HttpPost]
        public ActionResult CustomerReport(CustomerReportVM vmodel)
        {
            var model = _customerService.CustomerReport(vmodel);
            return View(model);
        }
        public ActionResult PaymentReport()
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            var model = new PaymentReportVM();
            return View(model);
        }
        [HttpPost]
        public ActionResult PaymentReport(PaymentReportVM vmodel)
        {
            var model = _paymentService.GetPaymentReport(vmodel);
            return View(model);
        }
        public ActionResult EarnedRevenueReport()
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            var model = new EarnedRevenueReportVM();
            return View(model);
        }
        [HttpPost]
        public ActionResult EarnedRevenueReport(EarnedRevenueReportVM vmodel)
        {
            var model = _paymentService.GetEarnedRevenueReport(vmodel);
            return View(model);
        }
        public ActionResult SharedRevenueReport()
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            var model = new SharedRevenueReportVM();
            return View(model);
        }
        [HttpPost]
        public ActionResult SharedRevenueReport(SharedRevenueReportVM vmodel)
        {
            var model = _paymentService.GetSharedRevenueReport(vmodel);
            return View(model);
        }
        // GET: Admin/Report

        #region Payment Report
        public ActionResult BillingInvoice(string billnumber)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Admin/Reports/Payment"), "BillInvoice.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                throw new Exception(String.Format("Report path not found in the specified directory: {0", path));
            }
            var header = _settingService.GetReportHeader();
            var customer = _paymentService.GetCustomerForReport(billnumber);
            var billServices = _paymentService.GetBillServices(billnumber);
            var billDetails = _paymentService.GetBillingDetails(billnumber);
            ReportDataSource Header = new ReportDataSource("SettingDataSet", header);
            ReportDataSource Customer = new ReportDataSource("CustomerDataSet", customer);
            ReportDataSource BilledService = new ReportDataSource("BillingDataSet", billServices);
            ReportDataSource BillDetails = new ReportDataSource("BillingDetailsDataSet", billDetails);
            if (Header != null && Customer != null && BilledService != null && BillDetails != null)
            {
                lr.DataSources.Add(Header);
                lr.DataSources.Add(Customer);
                lr.DataSources.Add(BilledService);
                lr.DataSources.Add(BillDetails);
            }
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                    "  <PageWidth>8.27in</PageWidth>" +
                    "  <PageHeight>11.69in</PageHeight>" +
                    "  <MarginTop>0.25in</MarginTop>" +
                    "  <MarginLeft>0.4in</MarginLeft>" +
                    "  <MarginRight>0.4in</MarginRight>" +
                    "  <MarginBottom>0.25in</MarginBottom>" +
                    "  <EmbedFonts>None</EmbedFonts>" +
                    "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedBytes, mimeType);
        }
        public ActionResult PaymentReciept(string recieptnumber, string billnumber)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Admin/Reports/Payment"), "PaymentReciept.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                throw new Exception(String.Format("Report path not found in the specified directory: {0", path));
            }
            var header = _settingService.GetReportHeader();
            var customer = _paymentService.GetCustomerForReport(billnumber);
            var billServices = _paymentService.GetBillServices(billnumber);
            var paymentDetail = _paymentService.GetPaymentDetails(recieptnumber);
            ReportDataSource Header = new ReportDataSource("SettingDataSet", header);
            ReportDataSource Customer = new ReportDataSource("CustomerDataSet", customer);
            ReportDataSource BilledService = new ReportDataSource("BillingDataSet", billServices);
            ReportDataSource PaymentDetails = new ReportDataSource("PaymentDataSet", paymentDetail);
            if (Header != null && Customer != null && BilledService != null && PaymentDetails!= null)
            {
                lr.DataSources.Add(Header);
                lr.DataSources.Add(Customer);
                lr.DataSources.Add(BilledService);
                lr.DataSources.Add(PaymentDetails);
            }
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                    "  <PageWidth>8.27in</PageWidth>" +
                    "  <PageHeight>11.69in</PageHeight>" +
                    "  <MarginTop>0.25in</MarginTop>" +
                    "  <MarginLeft>0.4in</MarginLeft>" +
                    "  <MarginRight>0.4in</MarginRight>" +
                    "  <MarginBottom>0.25in</MarginBottom>" +
                    "  <EmbedFonts>None</EmbedFonts>" +
                    "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedBytes, mimeType);
        }
        #endregion

        #region Laboratory report

        public ActionResult LabReport(string billnumber, int templateID, bool templated)
        {
            LocalReport lr = new LocalReport();
            string path = !templated ? Path.Combine(Server.MapPath("~/Areas/Admin/Reports/Laboratory"), "TemplatedLabResult.rdlc") : Path.Combine(Server.MapPath("~/Areas/Admin/Reports/Laboratory"), "NonTemplatedLabResult.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                throw new Exception(String.Format("Report path not found in the specified directory: {0}", path));
            }
            var header = _settingService.GetReportHeader();
            var customer = _paymentService.GetCustomerForReport(billnumber);
            var specimenCollection = _laboratoryService.GetSpecimenCollectedForReport(billnumber, templateID);
            ReportDataSource Header = new ReportDataSource("SettingDataSet", header);
            ReportDataSource Customer = new ReportDataSource("CustomerDataSet", customer);
            ReportDataSource SpecimenCollection = new ReportDataSource("SpecimenCollectionDataSet", specimenCollection);
            if (Header != null && Customer != null && SpecimenCollection != null)
            {
                lr.DataSources.Add(Header);
                lr.DataSources.Add(Customer);
                lr.DataSources.Add(SpecimenCollection);
            }

            if (!templated)
            {
                var templatedResult = _laboratoryService.GetTemplatedLabResultForReport(templateID, billnumber);
                ReportDataSource TemplatedLabResult = new ReportDataSource("TemplatedDataSet", templatedResult);
                if (TemplatedLabResult != null)
                {
                    lr.DataSources.Add(TemplatedLabResult);
                }
            }
            else
            {
                var nonTemplateResult = _laboratoryService.GetNonTemplatedLabPreparationForReport(billnumber);
                var nonTemplateOrganism = _laboratoryService.GetComputedOrganismXAntibiotics(nonTemplateResult.FirstOrDefault().Id);

                ReportDataSource NonTemplateResult = new ReportDataSource("NonTemplatedLabResultDataSet", nonTemplateResult);
                ReportDataSource NonTemplateOrganism = new ReportDataSource("NonTemplatedOrganismDataSet", nonTemplateOrganism);

                if (NonTemplateResult != null && NonTemplateOrganism != null)
                {
                    lr.DataSources.Add(NonTemplateResult);
                    lr.DataSources.Add(NonTemplateOrganism);
                }
            }
         
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                    "  <PageWidth>8.27in</PageWidth>" +
                    "  <PageHeight>11.69in</PageHeight>" +
                    "  <MarginTop>0.25in</MarginTop>" +
                    "  <MarginLeft>0.4in</MarginLeft>" +
                    "  <MarginRight>0.4in</MarginRight>" +
                    "  <MarginBottom>0.25in</MarginBottom>" +
                    "  <EmbedFonts>None</EmbedFonts>" +
                    "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedBytes, mimeType);
        }

        public ActionResult ViewResult(string billnumber)
        {
            var billedServices = _laboratoryService.GetServicesToPrepare(billnumber);
            var distinctServices = _laboratoryService.GetDistinctTemplateForBilledServices(billedServices);

            var response = new { distinctServices, billedServices };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region PDF Reports
        //[AllowAnonymous]
        //public ActionResult StaffOffDutyExportToPDF(OffDutyVM svm)
        //{
        //    byte[] bytes;
        //    string mimeType;
        //    using (var reportViewer = new ReportViewer())
        //    {
        //        reportViewer.ProcessingMode = ProcessingMode.Local;
        //        string path = Server.MapPath("~/Areas/Admin/Reports/StaffOffDutyReport.rdlc");
        //        reportViewer.LocalReport.ReportEmbeddedResource = path;
        //        reportViewer.LocalReport.ReportPath = path;
        //        reportViewer.LocalReport.EnableExternalImages = true;
        //        using (FileStream stream = new FileStream(path, FileMode.Open))
        //        {
        //            reportViewer.LocalReport.LoadReportDefinition(stream);
        //        }
        //        var StaffOffDutyDataset = _reportService.StaffOffDutyReport(svm).TableData;
        //        var ApplicationSettingsDataSet = _settingService.ApplicationSettings();
        //        if (StaffOffDutyDataset != null)
        //        {
        //            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", StaffOffDutyDataset));
        //            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", ApplicationSettingsDataSet));
        //        }
        //        Warning[] warnings;
        //        string[] streamids;
        //        string encoding;
        //        string extension;
        //        string deviceInfo = "<DeviceInfo><ColorDepth>32</ColorDepth><DpiX>350</DpiX><DpiY>350</DpiY><OutputFormat>EXCEL</OutputFormat>" +
        //"  <PageWidth>11in</PageWidth>" +
        // "  <PageHeight>8.5in</PageHeight>" +
        // "  <MarginTop>0.25in</MarginTop>" +
        // "  <MarginLeft>0.2in</MarginLeft>" +
        //  "  <MarginRight>0.2in</MarginRight>" +
        //  "  <MarginBottom>0.2in</MarginBottom>" +
        //"</DeviceInfo>";
        //        bytes = reportViewer.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
        //    }
        //    var cd = new System.Net.Mime.ContentDisposition
        //    {
        //        FileName = string.Format("StaffOffDutyReport.pdf"),
        //        Inline = true,
        //    };
        //    var result = new HttpResponseMessage(HttpStatusCode.OK);
        //    Stream stream1 = new MemoryStream(bytes);
        //    result.Content = new StreamContent(stream1);
        //    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
        //    return File(bytes, mimeType, "StaffOffDutyReport.pdf");
        //}
        #endregion
    }
}