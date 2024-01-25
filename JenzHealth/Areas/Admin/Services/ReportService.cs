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
        readonly IPaymentService _paymentService;
        public ReportService()
        {
            _db = new DatabaseEntities();
            _paymentService = new PaymentService();
        }

        public ReportService(DatabaseEntities db, PaymentService paymentService)
            
        {
            _db = db;
            _paymentService = paymentService;
        }

       
    }
}