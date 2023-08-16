using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.DAL.Entity
{
    public class MeasurementCollection
    {
        public int Id { get; set; }
        public string BillNumber { get; set; }
        public int ClothTypeMeasurementID { get; set; }
        public string Value { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey("ClothTypeMeasurementID")]
        public ClothTypeMeasurement ClothTypeMeasurement { get; set; }
    }
}
