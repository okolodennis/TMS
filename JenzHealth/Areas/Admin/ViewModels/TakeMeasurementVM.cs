using WebApp.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.ViewModels
{
    public class TakeMeasurementVM
    {
        public int Id { get; set; }
        public string CustomerUniqueID { get; set; }
        public string Gender { get; set; }
        public int Quantity { get; set; }
        public int ClothTypeID { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAge { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerGender { get; set; }
    }
}