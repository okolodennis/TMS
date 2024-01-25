using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.ViewModels
{

    public class ClothMeasurementReportVM
    {
        public int Id { get; set; }
        public string BillNumber { get; set; }
        public int ClothTypeId { get; set; }
        public string ClothType { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNumber { get; set; }
        public string Status { get; set; }
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        public string StartDateString { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        public string EndDateString { get; set; }
        public IList<dynamic> TableData { get; set; }
        public string exportfiletype { get; set; }
        public string caller { get; set; }
        public DateTime? Date { get; set; }
        public string TakenBy { get; set; }
        public ClothMeasurementReportVM()
        {

        }
        public ClothMeasurementReportVM(IEnumerable<dynamic> TData)
        {
            TableData = TData.ToArray();
        }
    }

}