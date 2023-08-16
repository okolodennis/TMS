using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.ViewModels
{
    public class SettlementSetupVM
    {
        public int Id { get; set; }
        public int ClothTypeID { get; set; }
        public int ClothTypeIDCreate { get; set; }
        public string ClothType{ get; set; }
        public string TailorName{ get; set; }
        public string Tailor{ get; set; }

        public int TailorIDCreate { get; set; }
        public int TailorID { get; set; }
        public decimal PartnerPercent { get; set; }
        public decimal OwnerPercent { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }

    }
}