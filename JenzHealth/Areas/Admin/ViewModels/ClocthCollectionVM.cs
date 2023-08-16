using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.ViewModels
{
    public class ClocthCollectionVM
    {
        public int Id { get; set; }
        public string CollectorName { get; set; }
        public string BillNumber { get; set; }
        public string ClothType { get; set; }
        public int ClothTypeID { get; set; }
        public DateTime? DateCollected { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CustomerName { get; set; }
    }
}