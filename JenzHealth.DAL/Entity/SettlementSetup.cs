using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.DAL.Entity
{
    public class SettlementSetup
    {
        [Key]
        public int Id { get; set; }
        public int ClothTypeID { get; set; }

        public int TailorID { get; set; }
        public decimal PartnerPercent { get; set; }
        public decimal OwnerPercent { get; set; }
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
        [ForeignKey("TailorID")]
        public User Tailor { get; set; }
    }
}
