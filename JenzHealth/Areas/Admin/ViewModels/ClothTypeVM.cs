using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.ViewModels
{
    public class ClothTypeVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameCreate { get; set; }
        public string CostPriceString { get; set; }
        public string SellingPriceString { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal CostPrice { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
    }
}