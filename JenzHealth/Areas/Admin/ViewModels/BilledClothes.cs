using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.ViewModels
{
    public class BilledClothes
    {
        public int BillId { get; set; }
        public int ClothTypeId { get; set; }
        public string ClothTypeName { get; set; }
        public int Quantity { get; set; }
    }
}