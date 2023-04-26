using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.ViewModels
{
    public class AntiBioticVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public int? OrganismID { get; set; }
        public string OrganismName { get; set; }
    }
}