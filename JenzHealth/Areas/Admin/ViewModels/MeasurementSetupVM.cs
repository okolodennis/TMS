using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.ViewModels
{
    public class TakeMeasurementVM
    {
        public string BillNumber { get; set; }
        public string CustomerName { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public List<MeasurementSetupVM> Setups { get; set; }
        public DateTime CollectionDate { get; set; } = DateTime.Now;
    }
    public class MeasurementSetupVM
    {
        public string BillNumber { get; set; }
        public int BillingID { get; set; }
        public int ClothTypeId { get; set; }
        public string ClothType { get; set; }
        public string Gender { get; set; }
        public int TotalQuantity { get; set; }
        public int Quantity { get; set; }
        public int BilledQuantity { get; set; }
        public string Tailor { get; set; }
        public int TailorId { get; set; }
        public DateTime CollectionDate { get; set; } = DateTime.Now;
        public List<ClothTypeParameter> Parameters { get; set; }
        public List<TailorAssignment> TailorAssignments { get; set; }
        public HttpPostedFileBase StyleImageFile { get; set; }
        public string StyleImageFileBase64 { get; set; }
        public HttpPostedFileBase FabricsImageFile { get; set; }
        public string FabricsImageFileBase64 { get; set; }
    }

    public class ClothTypeParameter
    {
        public int Id { get; set; }
        public int ParameterID { get; set; }
        public string Parameter { get; set; }
        public string Value { get; set; }
    }
    public class TailorAssignment
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string Tailor { get; set; }
        public int TailorId { get; set; }
        public DateTime CollectionDate { get; set; } = DateTime.Now;
    }
}