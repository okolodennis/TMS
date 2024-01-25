using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.ViewModels
{
    public class CustomerReportVM
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public string CustomerUniqueID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public string RegisteredBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        public string StartDateString { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        public string EndDateString { get; set; }
        public IList<CustomerReportVM> TableData { get; set; }
        public string exportfiletype { get; set; }
        public string caller { get; set; }
    }
}