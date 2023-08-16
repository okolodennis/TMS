using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.ViewModels.Report
{
    public class RequestTrackerVM
    {
        public int Id { get; set; }
        public string BillNumber { get; set; }
        public string CustomerName { get; set; }
        public bool HasCompletedPayment { get; set; }
        public bool ClothCollected { get; set; }
        public bool IsClothReady { get; set; }
        public DateTime? ClothCollectedOn { get; set; }
        public string ClothCollectedBy { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}