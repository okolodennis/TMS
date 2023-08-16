using WebApp.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.DAL.Entity;
using WebApp.Areas.Admin.ViewModels.Report;

namespace WebApp.Areas.Admin.Interfaces
{
    internal interface IActivityService
    {
        List<ClothStatusVM> GetClothesFromAssignTailor(string username);
        void UpdateClothesFromAssignTailor(List<ClothStatusVM> vmodel);
        TakeMeasurementVM SetupMeasurementCollection(string billNumber);
        void UpdateComputedMeasurement(List<MeasurementSetupVM> vmodel);
        List<RequestTrackerVM> TrackRequest(RequestTrackerVM vmodel);
        bool SaveClothCollector(RequestTrackerVM vmodel);
    }
}
