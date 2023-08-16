using WebApp.Areas.Admin.Interfaces;
using WebApp.Areas.Admin.ViewModels.Report;
using WebApp.DAL.DataConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using PowerfulExtensions.Linq;
using System.Web;

namespace WebApp.Areas.Admin.Services
{
    public class ReportService : IReportService
    {
        private readonly DatabaseEntities _db;
        readonly ISeedService _seedService;
        readonly IPaymentService _paymentService;
        readonly ILaboratoryService _laboratoryService;
        public ReportService()
        {
            _db = new DatabaseEntities();
            _seedService = new SeedService();
            _paymentService = new PaymentService();
            _laboratoryService = new LaboratoryService();
        }

        public ReportService(DatabaseEntities db, PaymentService paymentService, LaboratoryService laboratoryService, SeedService seedService)
        {
            _db = db;
            _seedService = seedService;
            _paymentService = paymentService;
            _laboratoryService = laboratoryService;
        }

       
    }
}