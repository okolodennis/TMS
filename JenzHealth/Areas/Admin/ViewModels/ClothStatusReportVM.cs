using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.ViewModels
{

    public class ClothStatusReportVM
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
        public string CustomerNumber { get; set; }
        public string Status { get; set; }
        public bool IsReady { get; set; }
        public string CollectionDateString { get; set; }
        public DateTime RecievedDate { get; set; }
        public DateTime CollectionDate { get; set; }
        public DateTime? ActualCollectionDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        public string StartDateString { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        public string EndDateString { get; set; }
        public IList<dynamic> TableData1 { get; set; }
        public IList<dynamic> TableData2 { get; set; }
        public IList<dynamic> TableData3 { get; set; }
        public string exportfiletype { get; set; }
        public string caller { get; set; }
        public ClothStatusReportVM()
        {

        }
        public ClothStatusReportVM(IEnumerable<dynamic> TData1, IEnumerable<dynamic> TData2, IEnumerable<dynamic> TData3)
        {
            TableData1 = TData1.ToArray();
            TableData2 = TData2.ToArray();
            TableData3 = TData3.ToArray();
        }
    }

}