﻿using WebApp.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.ViewModels
{
    public class BillingVM
    {
        public int Id { get; set; }
        public int CustomerID { get; set; }
        public string CustomerUniqueID { get; set; }
        public CustomerType CustomerType { get; set; }
        public string CustomerName { get; set; }
        public string CustomerGender { get; set; }
        public int CustomerAge { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public int ServiceID { get; set; }
        public int TailorID { get; set; }
        public string ServiceName { get; set; }
        public string InvoiceNumber { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime CollectionDate { get; set; }
        public int Quantity { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal SellingPrice { get; set; }
        public string SellingPriceString { get; set; }
        public string BillBy { get; set; }
    }
}