using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.ViewModels
{

    public class ClothStatusVM
    {
        public int Id { get; set; }
        public string Tailor { get; set; }
        public int TailorID { get; set; }
        public string BillNumber { get; set; }
        public int ClothTypeId { get; set; }
        public int Quantity { get; set; }
        public string ClothType { get; set; }
        public string Phone { get; set; }
        public string CustomerName { get; set; }
        public string Status { get; set; }
        public bool IsReady { get; set; }
    }

}