using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.DAL.Entity
{
    public  class JobAssignment
    {
        [Key]
        public int Id { get; set; }
        public int BookingID { get; set; }
        public int BillingID { get; set; }
        public int TailorID { get; set; }
        public int Quantity { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("BillingID")]
        public Billing Billing { get; set; }
        [ForeignKey("BookingID")]
        public Booking Booking { get; set; }
        [ForeignKey("TailorID")]
        public User Tailor { get; set; }
    }
}
