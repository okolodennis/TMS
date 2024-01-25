using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.DAL.Entity;

namespace WebApp.DAL.Entity
{
    public class AssignedTailorToBilledCloth
    {
        public int Id { get; set; }
        public int? BillingId { get; set; }
        public int TailorId { get; set; }
        public int? TakenById { get; set; }
        public int Quantity { get; set; }
        public DateTime DateAssigned { get; set; }
        public DateTime CollectionDate { get; set; }
        public bool IsReady { get; set; }
        public string StyleImageFile { get; set; }
        public string FabricsImageFile { get; set; }
        [ForeignKey("BillingId")]
        public virtual Billing Billing { get; set; }
        [ForeignKey("TailorId")]
        public virtual User Tailor { get; set; }
        [ForeignKey("TakenById")]
        public virtual User TakenBy { get; set; }
    }
}
