using WebApp.DAL.DataConnection;
using WebApp.DAL.Entity;
using WebApp.Areas.Admin.Helpers;
using WebApp.Areas.Admin.Interfaces;
using WebApp.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Areas.Admin.ViewModels.Report;
using System.Globalization;

namespace WebApp.Areas.Admin.Services
{
    public class ApplicationSettingsService : IApplicationSettingsService
    {
        /* Instancation of the database context model
         * and injecting some buisness layer services
         */
        #region Instanciation
        readonly DatabaseEntities _db;
        List<ClothTypeMeasurementVM> clothTypeMeasurementMainTableData = new List<ClothTypeMeasurementVM>();
        IEnumerable<ClothTypeMeasurement> clothTypeMeasurementTabledata;
        public ApplicationSettingsService()
        {
            _db = new DatabaseEntities();
        }
        public ApplicationSettingsService(DatabaseEntities db)
        {
            _db = db;
        }
        #endregion

        /* ********************************************************************************************************** */
        // Application settings

        // Getting the basic system application setting data
        public ApplicationSettingsVM GetApplicationSettings()
        {
            byte[] emptyArr = { 4, 3 };
            var model = _db.ApplicationSettings.FirstOrDefault();
            var Vmodel = new ApplicationSettingsVM()
            {
                ID = model.Id,
                AppName = model.AppName,
                Logo = model.Logo == null ? emptyArr : model.Logo,
                Watermark = model.Watermark == null ? emptyArr : model.Watermark,
                EnablePartPayment = model.EnablePartPayment,
                EnableSpecimentCollectedBy = model.EnableSpecimentCollectedBy,
                SalesRecieptCopyCount = model.SalesRecieptCopyCount,
                CustomerNumberPrefix = model.CustomerNumberPrefix,
                CodeGenSeed = model.CodeGenSeed,
                DepositeCount = model.DepositeCount,
                BillCount = model.BillCount,
                LabCount = model.LabCount,
                SessionTimeOut = model.SessionTimeOut,
                PaymentCount = model.PaymentCount,
                ShiftCount = model.ShiftCount,
                PaymentCompleted = model.PaymentCompleted,
                ClothReady = model.ClothReady
            };
            return Vmodel;
        }

        // Editting and updating the system application setting data
        public bool UpdateApplicationSettings(ApplicationSettingsVM Vmodel, HttpPostedFileBase Logo, HttpPostedFileBase Watermark)
        {
            bool hasSucceed = false;
            var model = _db.ApplicationSettings.FirstOrDefault(x => x.Id == Vmodel.ID);
            model.AppName = Vmodel.AppName;
            model.EnableSpecimentCollectedBy = Vmodel.EnableSpecimentCollectedBy;
            model.EnablePartPayment = Vmodel.EnablePartPayment;
            model.CustomerNumberPrefix = Vmodel.CustomerNumberPrefix;
            model.SalesRecieptCopyCount = Vmodel.SalesRecieptCopyCount;
            model.CodeGenSeed = Vmodel.CodeGenSeed;
            model.SessionTimeOut = Vmodel.SessionTimeOut;
            model.ClothReady = Vmodel.ClothReady;
            model.PaymentCompleted = Vmodel.PaymentCompleted;
            if (Logo != null)
                model.Logo = CustomSerializer.Serialize(Logo);
            if (Watermark != null)
                model.Watermark = CustomSerializer.Serialize(Watermark);
            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            var state = _db.SaveChanges();
            if (state > 0)
            {
                hasSucceed = true;
            }
            return hasSucceed;
        }

    public List<SettingsDataSetVM> GetReportHeader()
        {
            byte[] empty = { 4,3 };
            var response = _db.ApplicationSettings.Where(x => x.Id != 0).Select(b => new SettingsDataSetVM()
            {
               Id = b.Id,
               BrandName = b.AppName,
               InstitutionName = b.AppName,
               Logo = b.Logo == null ? empty : b.Logo,
            //   Watermark = b.Watermark == null ? empty : b.Watermark,
               DateGenerated = DateTime.Now
            }).ToList();
            return response;
        }

        // Creating ClothTypeMeasurement
        public bool CreateClothTypeMeasurement(ClothTypeMeasurementVM vmodel)
        {
            bool HasSaved = false;
            ClothTypeMeasurement model = new ClothTypeMeasurement()
            {
                ClothTypeID   = vmodel.ClothTypeIDCreate,
                MeasurementID = vmodel.MeasurementIDCreate,
                IsActive = true,
                IsDeleted = false,
                DateCreated = DateTime.Now,
                CreatedByID = Global.AuthenticatedUserID
            };
            _db.ClothTypeMeasurements.Add(model);
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Getting ClothTypeMeasurement
        public List<ClothTypeMeasurementVM> GetClothTypeMeasurement(ClothTypeMeasurementVM vmodel)
        {

            var model = _db.ClothTypeMeasurements.Where(x => (x.ClothTypeID == vmodel.ClothTypeID || x.MeasurementID == vmodel.MeasurementID) && x.IsDeleted == false).Select(b => new ClothTypeMeasurementVM()
            {
                Id = b.Id,
                ClothType = b.ClothType.Name,
                Measurement = b.Measurement.Name,
            }).ToList();
            return model;
        }

        // Editting and updating ClothTypeMeasurement
        public bool EditClothTypeMeasurement(ClothTypeMeasurementVM vmodel)
        {
            bool HasSaved = false;
            var model = _db.ClothTypeMeasurements.Where(x => x.Id == vmodel.Id).FirstOrDefault();
            model.ClothTypeID = vmodel.ClothTypeID;
            model.MeasurementID = vmodel.MeasurementID;
            model.ModifiedByID = Global.AuthenticatedUserID;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Deleting ClothTypeMeasurement
        public bool DeleteClothTypeMeasurement(int ID)
        {
            bool HasDeleted = false;
            var model = _db.ClothTypeMeasurements.Where(x => x.Id == ID).FirstOrDefault();
            model.IsDeleted = true;
            model.ModifiedByID = Global.AuthenticatedUserID;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasDeleted = true;
            return HasDeleted;
        }

        // Creating SettlementSetup
        public bool CreateSettlementSetup(SettlementSetupVM vmodel)
        {
            bool HasSaved = false;
            SettlementSetup model = new SettlementSetup()
            {
                ClothTypeID = vmodel.ClothTypeIDCreate,
                TailorID = vmodel.TailorIDCreate,
                PartnerPercent = vmodel.PartnerPercent,
                IsActive = true,
                IsDeleted = false,
                DateCreated = DateTime.Now,
                CreatedByID = Global.AuthenticatedUserID
            };
            _db.SettlementSetups.Add(model);
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Getting SettlementSetup
        public List<SettlementSetupVM> GetSettlementSetup(SettlementSetupVM vmodel)
        {

            var model = _db.SettlementSetups.Where(x => (x.ClothTypeID == vmodel.ClothTypeID || x.TailorID == vmodel.TailorID) && x.IsDeleted == false).Select(b => new SettlementSetupVM()
            {
                Id = b.Id,
                ClothType = b.ClothType.Name,
                Tailor = b.Tailor.Username,
                TailorName = b.Tailor.Lastname + " " + b.Tailor.Firstname,
                PartnerPercent = b.PartnerPercent,
            }).ToList();
            return model;
        }

        // Editting and updating SettlementSetup
        public bool EditSettlementSetup(SettlementSetupVM vmodel)
        {
            bool HasSaved = false;
            var model = _db.SettlementSetups.Where(x => x.Id == vmodel.Id).FirstOrDefault();
            model.PartnerPercent = vmodel.PartnerPercent;
            model.ModifiedByID = Global.AuthenticatedUserID;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Deleting SettlementSetup
        public bool DeleteSettlementSetup(int ID)
        {
            bool HasDeleted = false;
            var model = _db.SettlementSetups.Where(x => x.Id == ID).FirstOrDefault();
            model.IsDeleted = true;
            model.ModifiedByID = Global.AuthenticatedUserID;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasDeleted = true;
            return HasDeleted;
        }
        public SettlementSetupVM GetSettlementSetup(int Id)
        {
            return _db.SettlementSetups.Where(x => x.Id == Id).Select(o=> new SettlementSetupVM()
            { 
                Id = o.Id,
                Tailor = o.Tailor.Username,
                ClothType = o.ClothType.Name,
                PartnerPercent = o.PartnerPercent,
                OwnerPercent = o.OwnerPercent
            }).FirstOrDefault();
        }
        //public ClothTypeMeasurementVM GetClothTypeMeasurement(string ClothTypeMeasurementname)
        //{
        //    NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
        //    var model = _db.ClothTypeMeasurements.Where(x => x.Name == ClothTypeMeasurementname).Select(b => new ClothTypeMeasurementVM()
        //    {
        //        Id = b.Id,
        //        ClothTypeID = b.ClothTypeID,
        //        MeasurementID = b.MeasurementID,
        //        ClothType = b.ClothType.Name,
        //        Measurement = b.Measurement.Name,
        //    }).FirstOrDefault();

        //    //if (model != null)
        //    //{
        //    //    model.SellingPriceString = "₦" + model.SellingPrice.ToString("N", nfi);
        //    //    model.CostPriceString = "₦" + model.CostPrice.ToString("N", nfi);
        //    //}
        //    return model;
        //}
        public List<ClothTypeMeasurementVM> GetClothTypeMeasurementAutoComplete(string query)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            var model = _db.ClothTypeMeasurements.Where(x => x.ClothType.Name.StartsWith(query)).Select(b => new ClothTypeMeasurementVM()
            {
                Id = b.Id,
                ClothTypeID = b.ClothTypeID,
                MeasurementID = b.MeasurementID,
                ClothType = b.ClothType.Name,
                Measurement = b.Measurement.Name,
            }).ToList();
            return model;
        }
        public List<string> GetClothTypeMeasurementNameAutoComplete(string term)
        {
            List<string> ClothTypeMeasurements;
            ClothTypeMeasurements = _db.ClothTypeMeasurements.Where(x => x.IsDeleted == false && x.ClothType.Name.StartsWith(term)).Select(b => b.ClothType.Name).ToList();
            return ClothTypeMeasurements;
        }
        /* *************************************************************************** */
        //ClothType

        // Fetching ClothType
        public List<ClothTypeVM> GetClothTypes()
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            var model = _db.ClothTypes.Where(x => x.IsDeleted == false).Select(b => new ClothTypeVM()
            {
                Id = b.Id,
                Name = b.Name,
                CostPrice = b.CostPrice
            }).ToList();
            foreach (var each in model)
            {
                each.CostPriceString = "₦" + each.CostPrice.ToString("N", nfi);
            }
            return model;
        }
        public ClothTypeVM GetClothType(string clothTypeName)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            var model = _db.ClothTypes.Where(x => x.Name == clothTypeName && !x.IsDeleted).Select(b => new ClothTypeVM()
            {
                Id = b.Id,
                Name = b.Name,
                CostPrice = b.CostPrice,
            }).FirstOrDefault();

            if (model != null)
            {
                model.CostPriceString = "₦" + model.CostPrice.ToString("N", nfi);
            }
            return model;
        }
        // Creating ClothType
        public bool CreateClothType(ClothTypeVM vmodel)
        {
            bool HasSaved = false;
            ClothType model = new ClothType()
            {
                Name = vmodel.NameCreate,
                CostPrice = CustomSerializer.UnMaskString(vmodel.CostPriceString),
                IsDeleted = false,
                DateCreated = DateTime.Now,
                CreatedByID = Global.AuthenticatedUserID,
            };
            _db.ClothTypes.Add(model);
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Getting ClothType
        public ClothTypeVM GetClothType(int ID)
        {
            var model = _db.ClothTypes.Where(x => x.Id == ID).Select(b => new ClothTypeVM()
            {
                Id = b.Id,
                Name = b.Name,
                CostPriceString = b.CostPrice.ToString()
            }).FirstOrDefault();
            return model;
        }

        // Editting and updating ClothType
        public bool EditClothType(ClothTypeVM vmodel)
        {
            bool HasSaved = false;
            var model = _db.ClothTypes.Where(x => x.Id == vmodel.Id).FirstOrDefault();
            model.Name = vmodel.Name;
            model.CostPrice = CustomSerializer.UnMaskString(vmodel.CostPriceString);
            model.ModifiedByID = Global.AuthenticatedUserID;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Deleting ClothType
        public bool DeleteClothType(int ID)
        {
            bool HasDeleted = false;
            var model = _db.ClothTypes.Where(x => x.Id == ID).FirstOrDefault();
            model.IsDeleted = true;
            model.ModifiedByID = Global.AuthenticatedUserID;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasDeleted = true;
            return HasDeleted;
        }
        public List<string> GetClothTypeAutoComplete(string term)
        {
            List<string> services;
            services = _db.ClothTypes.Where(x => x.IsDeleted == false && x.Name.StartsWith(term)).Select(b => b.Name).ToList();
            return services;
        }
        /* *************************************************************************** */
        //Measurement

        // Fetching Measurement
        public List<MeasurementVM> GetMeasurement()
        {
            var model = _db.Measurements.Where(x => x.IsDeleted == false).Select(b => new MeasurementVM()
            {
                Id = b.Id,
                Name = b.Name,
            }).ToList();
            return model;
        }

        // Creating Measurement
        public bool CreateMeasurement(MeasurementVM vmodel)
        {
            bool HasSaved = false;
            Measurement model = new Measurement()
            {
                Name = vmodel.NameCreate,
                IsDeleted = false,
                DateCreated = DateTime.Now,
                CreatedByID = Global.AuthenticatedUserID,
            };
            _db.Measurements.Add(model);
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Getting Measurement
        public MeasurementVM GetMeasurement(int ID)
        {
            var model = _db.Measurements.Where(x => x.Id == ID).Select(b => new MeasurementVM()
            {
                Id = b.Id,
                Name = b.Name,
            }).FirstOrDefault();
            return model;
        }

        // Editting and updating Measurement
        public bool EditMeasurement(MeasurementVM vmodel)
        {
            bool HasSaved = false;
            var model = _db.Measurements.Where(x => x.Id == vmodel.Id).FirstOrDefault();
            model.Name = vmodel.Name;
            model.ModifiedByID = Global.AuthenticatedUserID;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Deleting Measurement
        public bool DeleteMeasurement(int ID)
        {
            bool HasDeleted = false;
            var model = _db.Measurements.Where(x => x.Id == ID).FirstOrDefault();
            model.IsDeleted = true;
            model.ModifiedByID = Global.AuthenticatedUserID;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasDeleted = true;
            return HasDeleted;
        }
        public bool CheckIfClothTypeExist(string term)
        {
            var exist = _db.ClothTypes.Any(x => x.Name == term && !x.IsDeleted && x.IsActive);
            return exist;
        }
        public bool CheckIfMeasurementExist(string term)
        {
            var exist = _db.Measurements.Any(x => x.Name == term && !x.IsDeleted && x.IsActive);
            return exist;
        }
        public bool CheckIfClothTypeMeasurementExist(int clothtype, int measurement)
        {
            var exist = _db.ClothTypeMeasurements.Any(x => x.ClothTypeID == clothtype && x.MeasurementID == measurement && !x.IsDeleted && x.IsActive);
            return exist;
        }
        public bool CheckIfSettlementSetupExist(int clothtype, int tailor)
        {
            var exist = _db.SettlementSetups.Any(x => x.ClothTypeID == clothtype && x.TailorID == tailor && !x.IsDeleted && x.IsActive);
            return exist;
        }
    }
}