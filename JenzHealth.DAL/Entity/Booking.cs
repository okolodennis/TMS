using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.DAL.Entity
{
    public  class Booking
    {
        [Key]
        public int Id { get; set; }
        public int TailorID { get; set; }
        public string BillInvoiceNumber { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CollectionDate { get; set; }
        public DateTime DateCreated { get; set; }

       
      
      
    }
}
