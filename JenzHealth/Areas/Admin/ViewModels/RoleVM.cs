using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.ViewModels
{
    public class RoleVM
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool EnableShift { get; set; }
        public int ShiftExpiration { get; set; }
    }
}