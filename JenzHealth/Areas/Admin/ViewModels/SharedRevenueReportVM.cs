using WebApp.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.ViewModels
{
    public class SharedRevenueReportVM
    {
        public int Id { get; set; }
        public int ClothTypeID { get; set; }
        public string ClothType { get; set; }
        public int Quantity { get; set; }
        public string BillNumber { get; set; }
        public string FinalReceiptNo { get; set; }
        public DateTime Date { get; set; }
        public DateTime? StartDate { get; set; }
        public string StartDateString { get; set; }
        public DateTime? EndDate { get; set; }
        public string EndDateString { get; set; }
        public string Tailor { get; set; }
        public int TailorID { get; set; }
        public string TailorName { get; set; }
        public decimal TotalCharge { get; set; }
        public decimal OwnerShare { get; set; }
        public decimal PartnerShare { get; set; }
        public decimal UnitCharge { get; set; }
        public decimal TotalUnitCharge { get; set; }
        public decimal TotalTotalCharge { get; set; }
        public decimal TotalPartnerShare { get; set; }
        public decimal TotalOwnerShare { get; set; }
        public decimal TotalQuantity { get; set; }
        public IList<SharedRevenueReportVM> TableData { get; set; }
    }
}