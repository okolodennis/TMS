using WebApp.Areas.Admin.ViewModels;
using WebApp.Areas.Admin.ViewModels.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WebApp.Areas.Admin.Interfaces
{
    interface IApplicationSettingsService
    {
        // Application settings
        ApplicationSettingsVM GetApplicationSettings();
        bool UpdateApplicationSettings(ApplicationSettingsVM Vmodel, HttpPostedFileBase Logo, HttpPostedFileBase Watermark);
        List<SettingsDataSetVM> GetReportHeader();
        bool CreateClothTypeMeasurement(ClothTypeMeasurementVM vmodel);
        List<ClothTypeMeasurementVM> GetClothTypeMeasurement();
        bool EditClothTypeMeasurement(ClothTypeMeasurementVM vmodel);
        bool DeleteClothTypeMeasurement(int ID);
        ClothTypeMeasurementVM GetClothTypeMeasurement(string ClothTypeMeasurementname);
        List<ClothTypeMeasurementVM> GetClothTypeMeasurementAutoComplete(string query);
        List<string> GetClothTypeMeasurementNameAutoComplete(string term);
        List<ClothTypeVM> GetClothTypes();
        ClothTypeVM GetClothType(string clothTypeName);
        bool CreateClothType(ClothTypeVM vmodel);
        ClothTypeVM GetClothType(int ID);
        bool EditClothType(ClothTypeVM vmodel);
        bool DeleteClothType(int ID);
        List<MeasurementVM> GetMeasurement();
        bool CreateMeasurement(MeasurementVM vmodel);
        MeasurementVM GetMeasurement(int ID);
        bool EditMeasurement(MeasurementVM vmodel);
        bool DeleteMeasurement(int ID);
        List<string> GetClothTypeAutoComplete(string term);
    }
}
