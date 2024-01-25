using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.ViewModels
{
    public class CustomerMeasurementVM
    {
        public int Id { get; set; }
        public string BillNumber { get; set; }
        public int ClothTypeId { get; set; }
        public string ClothType { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNumber { get; set; }
        public string Measurement { get; set; }
        public string Parameter { get; set; }
        public string Value { get; set; }
        public string Date { get; set; }
        public DateTime DateCreated { get; set; }

    }
}