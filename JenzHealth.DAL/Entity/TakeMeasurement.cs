using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.DAL.Entity
{
    public class TakeMeasurement
    {
        public int Id { get; set; }
        public string CustomerUniqueID { get; set; }
        public string DepositeReciept { get; set; }
        public string Gender { get; set; }
        public int Quantity { get; set; }
        public int ClothTypeID { get; set; }
        public int CustomerID { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedByID { get; set; }
        public int? ModifiedByID { get; set; }
        [ForeignKey("ModifiedByID")]
        public User ModifiedBy { get; set; }
        [ForeignKey("CreatedByID")]
        public User CreatedBy { get; set; }
        [ForeignKey("CustomerID")]
        public Customer Customer { get; set; }
    }
}
