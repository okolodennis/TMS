using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.DAL.Entity
{
    public class ClothTypeMeasurement
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int ClothTypeID { get; set; }

        public int MeasurementID { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive{ get; set; }
        public DateTime DateCreated { get; set; }
        public int? CreatedByID { get; set; }
        public int? ModifiedByID { get; set; }
        [ForeignKey("CreatedByID")]
        public User CreatedBy { get; set; }
        [ForeignKey("ModifiedByID")]
        public User ModifiedBy { get; set; }
        [ForeignKey("ClothTypeID")]
        public ClothType ClothType { get; set; }
        [ForeignKey("MeasurementID")]
        public Measurement Measurement { get; set; }
    }
}
