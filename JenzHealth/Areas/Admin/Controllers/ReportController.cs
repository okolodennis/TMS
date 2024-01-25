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
        IUserService _userService;
        IReportService _reportService;
        IActivityService _activityService;
        public ReportController()
        {
            _customerService = new CustomerService(db);
            _paymentService = new PaymentService(db, new UserService());
            _settingService = new ApplicationSettingsService(db);
            _userService = new UserService(db);
            _activityService = new ActivityService(db);
            _reportService = new ReportService(db, new PaymentService());
        }
        public ReportController(
            CustomerService customerService,
            PaymentService paymentService,
            ApplicationSettingsService settingService,
            UserService userService,
            ReportService reportService,
           ActivityService activityService
            )
        {
            _customerService = customerService;
            _paymentService = paymentService;
            _settingService = settingService;
            _userService = userService;
            _reportService = reportService;
            _activityService = activityService;
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
            if (vmodel.exportfiletype != "" && vmodel.exportfiletype != null)
            {
                vmodel.caller = "Export";
                if (vmodel.exportfiletype == "PDF")
                {
                    return RedirectToAction("CustomerExportToPDF", vmodel);
                }
            }
            else
            {
                return View(_customerService.CustomerReport(vmodel));
            }
            return View(vmodel);
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
            if (vmodel.exportfiletype != "" && vmodel.exportfiletype != null)
            {
                vmodel.caller = "Export";
                if (vmodel.exportfiletype == "PDF")
                {
                    return RedirectToAction("PaymentExportToPDF", vmodel);
                }
            }
            else
            {
                return View(_paymentService.GetPaymentReport(vmodel));
            }
            return View(vmodel);
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
            if (vmodel.exportfiletype != "" && vmodel.exportfiletype != null)
            {
                vmodel.caller = "Export";
                if (vmodel.exportfiletype == "PDF")
                {
                    return RedirectToAction("EarnedRevenueExportToPDF", vmodel);
                }
            }
            else
            {
                return View(_paymentService.GetEarnedRevenueReport(vmodel));
            }
            return View(vmodel);
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
            if (vmodel.exportfiletype != "" && vmodel.exportfiletype != null)
            {
                vmodel.caller = "Export";
                if (vmodel.exportfiletype == "PDF")
                {
                    return RedirectToAction("SharedRevenueExportToPDF", vmodel);
                }
            }
            else
            {
                return View(_paymentService.GetSharedRevenueReport(vmodel));
            }
            return View(vmodel);
        }
        public ActionResult ClothStatusReport()
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            var model = new ClothStatusReportVM();
            return View(model);
        }
        [HttpPost]
        public ActionResult ClothStatusReport(ClothStatusReportVM vmodel)
        {
            if (vmodel.exportfiletype != "" && vmodel.exportfiletype != null)
            {
                vmodel.caller = "Export";
                if (vmodel.exportfiletype == "PDF")
                {
                    return RedirectToAction("CustomerExportToPDF", vmodel);
                }
            }
            else
            {
                return View(_activityService.GetClothStatusReport(vmodel));
            }
            return View(vmodel);
        }
        public ActionResult ClothMeasurementReport()
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            ViewBag.ClothType = new SelectList(db.ClothTypes.Where(x => x.IsDeleted == false), "Id", "Name");
            var model = new ClothMeasurementReportVM();
            return View(model);
        }
        [HttpPost]
        public ActionResult ClothMeasurementReport(ClothMeasurementReportVM vmodel)
        {
            if (vmodel.exportfiletype != "" && vmodel.exportfiletype != null)
            {
                vmodel.caller = "Export";
                if (vmodel.exportfiletype == "PDF")
                {
                    ViewBag.ClothType = new SelectList(db.ClothTypes.Where(x => x.IsDeleted == false), "Id", "Name");
                    return RedirectToAction("CustomerMeasurementExportToPDF", vmodel);
                }
            }
            else
            {
            ViewBag.ClothType = new SelectList(db.ClothTypes.Where(x => x.IsDeleted == false), "Id", "Name");
                return View(_activityService.GetClothMeasurementReport(vmodel));
            }
            ViewBag.ClothType = new SelectList(db.ClothTypes.Where(x => x.IsDeleted == false), "Id", "Name");
            return View(vmodel);
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

     
        #region PDF Reports
        [AllowAnonymous]
        public ActionResult CustomerExportToPDF(CustomerReportVM svm)
        {
            byte[] bytes;
            string mimeType;
            using (var reportViewer = new ReportViewer())
            {
                reportViewer.ProcessingMode = ProcessingMode.Local;
                string path = Server.MapPath("~/Areas/Admin/Reports/CustomerReport.rdlc");
                reportViewer.LocalReport.ReportEmbeddedResource = path;
                reportViewer.LocalReport.ReportPath = path;
                reportViewer.LocalReport.EnableExternalImages = true;
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    reportViewer.LocalReport.LoadReportDefinition(stream);
                }
                var mainDataSet = _customerService.CustomerReport(svm).TableData;
                var ApplicationSettingsDataSet = _settingService.GetReportHeader();
                if (mainDataSet != null)
                {
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", mainDataSet));
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", ApplicationSettingsDataSet));
                }
                Warning[] warnings;
                string[] streamids;
                string encoding;
                string extension;
                string deviceInfo = "<DeviceInfo><ColorDepth>32</ColorDepth><DpiX>350</DpiX><DpiY>350</DpiY><OutputFormat>PDF</OutputFormat>" +
        "  <PageWidth>11in</PageWidth>" +
         "  <PageHeight>8.5in</PageHeight>" +
         "  <MarginTop>0.25in</MarginTop>" +
         "  <MarginLeft>0.2in</MarginLeft>" +
          "  <MarginRight>0.2in</MarginRight>" +
          "  <MarginBottom>0.2in</MarginBottom>" +
        "</DeviceInfo>";
                bytes = reportViewer.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
            }
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = string.Format("CustomerReport.pdf"),
                Inline = true,
            };
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            Stream stream1 = new MemoryStream(bytes);
            result.Content = new StreamContent(stream1);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            return File(bytes, mimeType);
        }

        [AllowAnonymous]
        public ActionResult PaymentExportToPDF(PaymentReportVM svm)
        {
            byte[] bytes;
            string mimeType;
            using (var reportViewer = new ReportViewer())
            {
                reportViewer.ProcessingMode = ProcessingMode.Local;
                string path = Server.MapPath("~/Areas/Admin/Reports/PaymentReport.rdlc");
                reportViewer.LocalReport.ReportEmbeddedResource = path;
                reportViewer.LocalReport.ReportPath = path;
                reportViewer.LocalReport.EnableExternalImages = true;
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    reportViewer.LocalReport.LoadReportDefinition(stream);
                }
                var mainDataSet = _paymentService.GetPaymentReport(svm).TableData;
                var ApplicationSettingsDataSet = _settingService.GetReportHeader();
                if (mainDataSet != null)
                {
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", mainDataSet));
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", ApplicationSettingsDataSet));
                }
                Warning[] warnings;
                string[] streamids;
                string encoding;
                string extension;
                string deviceInfo = "<DeviceInfo><ColorDepth>32</ColorDepth><DpiX>350</DpiX><DpiY>350</DpiY><OutputFormat>PDF</OutputFormat>" +
        "  <PageWidth>11in</PageWidth>" +
         "  <PageHeight>8.5in</PageHeight>" +
         "  <MarginTop>0.25in</MarginTop>" +
         "  <MarginLeft>0.2in</MarginLeft>" +
          "  <MarginRight>0.2in</MarginRight>" +
          "  <MarginBottom>0.2in</MarginBottom>" +
        "</DeviceInfo>";
                bytes = reportViewer.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
            }
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = string.Format("PaymentReport.pdf"),
                Inline = true,
            };
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            Stream stream1 = new MemoryStream(bytes);
            result.Content = new StreamContent(stream1);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            return File(bytes, mimeType);
        }
        [AllowAnonymous]
        public ActionResult SharedRevenueExportToPDF(SharedRevenueReportVM svm)
        {
            byte[] bytes;
            string mimeType;
            using (var reportViewer = new ReportViewer())
            {
                reportViewer.ProcessingMode = ProcessingMode.Local;
                string path = Server.MapPath("~/Areas/Admin/Reports/SharedRevenueReport.rdlc");
                reportViewer.LocalReport.ReportEmbeddedResource = path;
                reportViewer.LocalReport.ReportPath = path;
                reportViewer.LocalReport.EnableExternalImages = true;
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    reportViewer.LocalReport.LoadReportDefinition(stream);
                }
                var mainDataSet = _paymentService.GetSharedRevenueReport(svm).TableData;
                var ApplicationSettingsDataSet = _settingService.GetReportHeader();
                if (mainDataSet != null)
                {
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", mainDataSet));
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", ApplicationSettingsDataSet));
                }
                Warning[] warnings;
                string[] streamids;
                string encoding;
                string extension;
                string deviceInfo = "<DeviceInfo><ColorDepth>32</ColorDepth><DpiX>350</DpiX><DpiY>350</DpiY><OutputFormat>PDF</OutputFormat>" +
        "  <PageWidth>11in</PageWidth>" +
         "  <PageHeight>8.5in</PageHeight>" +
         "  <MarginTop>0.25in</MarginTop>" +
         "  <MarginLeft>0.2in</MarginLeft>" +
          "  <MarginRight>0.2in</MarginRight>" +
          "  <MarginBottom>0.2in</MarginBottom>" +
        "</DeviceInfo>";
                bytes = reportViewer.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
            }
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = string.Format("SharedRevenueReport.pdf"),
                Inline = true,
            };
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            Stream stream1 = new MemoryStream(bytes);
            result.Content = new StreamContent(stream1);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            return File(bytes, mimeType);
        }
        [AllowAnonymous]
        public ActionResult EarnedRevenueExportToPDF(EarnedRevenueReportVM svm)
        {
            byte[] bytes;
            string mimeType;
            using (var reportViewer = new ReportViewer())
            {
                reportViewer.ProcessingMode = ProcessingMode.Local;
                string path = Server.MapPath("~/Areas/Admin/Reports/EarnedRevenueReport.rdlc");
                reportViewer.LocalReport.ReportEmbeddedResource = path;
                reportViewer.LocalReport.ReportPath = path;
                reportViewer.LocalReport.EnableExternalImages = true;
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    reportViewer.LocalReport.LoadReportDefinition(stream);
                }
                var mainDataSet = _paymentService.GetEarnedRevenueReport(svm).TableData;
                var ApplicationSettingsDataSet = _settingService.GetReportHeader();
                if (mainDataSet != null)
                {
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", mainDataSet));
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", ApplicationSettingsDataSet));
                }
                Warning[] warnings;
                string[] streamids;
                string encoding;
                string extension;
                string deviceInfo = "<DeviceInfo><ColorDepth>32</ColorDepth><DpiX>350</DpiX><DpiY>350</DpiY><OutputFormat>PDF</OutputFormat>" +
        "  <PageWidth>11in</PageWidth>" +
         "  <PageHeight>8.5in</PageHeight>" +
         "  <MarginTop>0.25in</MarginTop>" +
         "  <MarginLeft>0.2in</MarginLeft>" +
          "  <MarginRight>0.2in</MarginRight>" +
          "  <MarginBottom>0.2in</MarginBottom>" +
        "</DeviceInfo>";
                bytes = reportViewer.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
            }
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = string.Format("EarnedRevenueReport.pdf"),
                Inline = true,
            };
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            Stream stream1 = new MemoryStream(bytes);
            result.Content = new StreamContent(stream1);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            return File(bytes, mimeType);
        }
        [AllowAnonymous]
        public ActionResult CustomerMeasurementExportToPDF(string CustomerNumber, int ClothTypeID, DateTime StartDate, DateTime EndDate)
        {
            byte[] bytes;
            string mimeType;
            using (var reportViewer = new ReportViewer())
            {
                reportViewer.ProcessingMode = ProcessingMode.Local;
                string path = Server.MapPath("~/Areas/Admin/Reports/ClothMeasurementReport.rdlc");
                reportViewer.LocalReport.ReportEmbeddedResource = path;
                reportViewer.LocalReport.ReportPath = path;
                reportViewer.LocalReport.EnableExternalImages = true;
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    reportViewer.LocalReport.LoadReportDefinition(stream);
                }
               // var mainDataSet = new List<CustomerMeasurementVM>();
                var mainDataSet = _activityService.GetCustomerMeasurement(CustomerNumber, ClothTypeID, StartDate, EndDate);
                var ApplicationSettingsDataSet = _settingService.GetReportHeader();
                if (mainDataSet != null)
                {
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", mainDataSet));
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", ApplicationSettingsDataSet));
                }
                Warning[] warnings;
                string[] streamids;
                string encoding;
                string extension;
                string deviceInfo = "<DeviceInfo><ColorDepth>32</ColorDepth><DpiX>350</DpiX><DpiY>350</DpiY><OutputFormat>PDF</OutputFormat>" +
        "  <PageWidth>11in</PageWidth>" +
         "  <PageHeight>8.5in</PageHeight>" +
         "  <MarginTop>0.25in</MarginTop>" +
         "  <MarginLeft>0.2in</MarginLeft>" +
          "  <MarginRight>0.2in</MarginRight>" +
          "  <MarginBottom>0.2in</MarginBottom>" +
        "</DeviceInfo>";
                bytes = reportViewer.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
            }
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = string.Format("CustomerMeasurement.pdf"),
                Inline = true,
            };
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            Stream stream1 = new MemoryStream(bytes);
            result.Content = new StreamContent(stream1);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            return File(bytes, mimeType);
        }
        #endregion
    }
}