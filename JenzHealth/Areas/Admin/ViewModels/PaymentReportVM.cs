using WebApp.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.Admin.ViewModels
{
    public class PaymentReportVM
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public int ClothTypeID { get; set; }
        public string ClothType { get; set; }
        public int Quantity { get; set; }
        public string CustomerName { get; set; }
        public string BillNumber { get; set; }
        public string ReceiptNo { get; set; }
        public DateTime Date { get; set; }
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        public string StartDateString { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        public string EndDateString { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Cash { get; set; }
        public decimal TotalCash { get; set; }
        public decimal POS { get; set; }
        public decimal TotalPOS { get; set; }
        public decimal ETransfer { get; set; }
        public decimal TotalETransfer { get; set; }
        public decimal CumulativeAmount { get; set; }
        public decimal TotalCumulativeAmount { get; set; }
        public IList<PaymentReportVM> TableData { get; set; }
        public string exportfiletype { get; set; }
        public string caller { get; set; }
    }
}