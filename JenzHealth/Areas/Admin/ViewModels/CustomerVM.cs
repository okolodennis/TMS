﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.ViewModels
{
    public class CustomerVM
    {
        public int Id { get; set; }
        public string CustomerUniqueID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public string Religion { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
      
    }
}