using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.ViewModels
{
    public class ClothTypeMeasurementVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ClothTypeID { get; set; }
        public string ClothType{ get; set; }

        public int MeasurementID { get; set; }
        public string Measurement { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
    }
}