using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.ViewModels.Report
{
    public class SettingsDataSetVM
    {
        public int Id { get; set; }
        public string BrandName { get; set; }
        public string InstitutionName { get; set; }
        public byte[] Logo { get; set; }
        public byte[] Watermark { get; set; }
        public DateTime DateGenerated { get; set; }
    }
}