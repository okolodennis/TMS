using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.ViewModels.Report
{
    public class RequestTrackerVM
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalDeposit { get; set; }
        public decimal Balance { get; set; }
        public string BillNumber { get; set; }
        public string ReceiptNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerUniqueID { get; set; }
        public bool HasCompletedPayment { get; set; }
        public bool ClothCollected { get; set; }
        public bool IsClothReady { get; set; }
        public DateTime? ClothCollectedOn { get; set; }
        public string ClothCollectedBy { get; set; }
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
    }
}