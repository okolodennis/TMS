using WebApp.Areas.Admin.Interfaces;
using WebApp.Areas.Admin.ViewModels;
using WebApp.DAL.DataConnection;
using WebApp.DAL.Entity;
using System;
using PowerfulExtensions.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Areas.Admin.ViewModels.Report;

namespace WebApp.Areas.Admin.Services
{
    public class ActivityService : IActivityService
    {

        /* Instancation of the database context model
      * and injecting some buisness layer services
      */
        #region Instanciation
        readonly DatabaseEntities _db;
        readonly IPaymentService _paymentService;
        readonly IUserService _userService;
        private readonly ILaboratoryService _laboratoryService;

        public ActivityService()
        {
            _db = new DatabaseEntities();
            _userService = new UserService();
            _laboratoryService = new LaboratoryService();
        }
        public ActivityService(DatabaseEntities db, LaboratoryService laboratoryService)
        {
            _db = db;
            _userService = new UserService(db);
            _paymentService = new PaymentService();
            _laboratoryService = laboratoryService;
        }
        #endregion

        /* *************************************************************************** */
        //Cloth .


        public List<ClothStatusVM> GetClothesFromAssignTailor(string username)
        {
            var tailor = _db.Users.FirstOrDefault(x => x.Username == username && !x.IsDeleted && x.IsActive);
            if (tailor != null)
            {
                if (Global.AuthenticatedUserID == tailor.Id || Global.AuthenticatedUserRoleID != 3)
                {
                    var model = _db.AssignedTailorToBilledClothes.Where(x => x.TailorId == tailor.Id).OrderByDescending(o => o.Id).Select(b => new ClothStatusVM()
                    {
                        Id = b.Id,
                        CustomerName = _db.Billings.FirstOrDefault(o => o.InvoiceNumber == b.Billing.InvoiceNumber).CustomerName,
                        Phone = _db.Billings.FirstOrDefault(o => o.InvoiceNumber == b.Billing.InvoiceNumber).CustomerPhoneNumber,
                        BillNumber = b.Billing.InvoiceNumber,
                        ClothType = b.Billing.ClothType.Name,
                        Quantity = b.Quantity,
                        Status = b.IsReady ? "Ready" : "Not Ready",
                        IsReady = b.IsReady,
                    }).ToList();

                    return model;
                }
                else
                {
                    return new List<ClothStatusVM>();
                }
            }
            else
            {
                return new List<ClothStatusVM>();
            }
        }
        public void UpdateClothesFromAssignTailor(List<ClothStatusVM> vmodel)
        {
            foreach (var item in vmodel)
            {
                var record = _db.AssignedTailorToBilledClothes.FirstOrDefault(x => x.Id == item.Id);
                record.IsReady = item.IsReady;

                _db.Entry(record).State = System.Data.Entity.EntityState.Modified;
            }
            _db.SaveChanges();
        }
        //Take Measurement

        public List<BilledClothes> GetClothesToTakeMeasurement(string invoiceNumber)
        {
            var model = _db.Billings.Where(x => x.InvoiceNumber == invoiceNumber && x.IsDeleted == false).Select(b => new BilledClothes()
            {
                BillId = b.Id,
                ClothTypeId = b.ClothTypeID,
                ClothTypeName = b.ClothType.Name,
                Quantity = b.Quantity,
            }).ToList();

            return model;
        }

        public ClothTypeParameter GetParamaterValue(string billnumber, int clothTypeMeasurementId)
        {
            var record = _db.MeasurementCollections.Where(x => x.BillNumber == billnumber && x.ClothTypeMeasurementID == clothTypeMeasurementId)
                .Select(b => new ClothTypeParameter()
                {
                    Id = b.Id,
                    Value = b.Value,
                }).FirstOrDefault();
            if (record != null)
                return record;
            else
                return new ClothTypeParameter();
        }
        public TakeMeasurementVM SetupMeasurementCollection(string billNumber)
        {
            var model = new TakeMeasurementVM();
            var customer = _paymentService.GetCustomerForBill(billNumber);
            model.CustomerName = customer.CustomerName;
            model.BillNumber = customer.InvoiceNumber;
            model.PhoneNumber = customer.CustomerPhoneNumber;
            model.Gender = customer.CustomerGender;


            List<MeasurementSetupVM> setups = new List<MeasurementSetupVM>();

            var billedClothTypes = this.GetClothesToTakeMeasurement(billNumber);

            foreach (var clothtype in billedClothTypes)
            {
                MeasurementSetupVM measurementSetup = new MeasurementSetupVM
                {
                    BillingID = clothtype.BillId,
                    BilledQuantity = clothtype.Quantity,
                    ClothTypeId = clothtype.ClothTypeId,
                    ClothType = clothtype.ClothTypeName,
                    Parameters = new List<ClothTypeParameter>()
                };

                var parameterSetups = _db.ClothTypeMeasurements.Where(x => x.ClothTypeID == clothtype.ClothTypeId && x.IsDeleted == false).Select(b => new ClothTypeParameter()
                {
                    Id = b.Id,
                    Parameter = b.Measurement.Name,
                    ParameterID = b.MeasurementID
                }).ToList();
                int measurementCollectionId = 0;
                // Check for database record and map
                foreach (var parametersetup in parameterSetups)
                {
                    var record = this.GetParamaterValue(billNumber, parametersetup.Id);
                    parametersetup.Value = record.Value;
                    measurementCollectionId = record.Id;
                }

                // Check for assigned tailor
                var assignedTailor = _db.AssignedTailorToBilledClothes.FirstOrDefault(x => x.Billing.Id == measurementSetup.BillingID);
                if (assignedTailor != null)
                {
                    measurementSetup.BillingID = measurementSetup.BillingID;
                    measurementSetup.TailorId = assignedTailor.TailorId;
                    measurementSetup.Tailor = _db.Users.FirstOrDefault(x => x.Id == assignedTailor.TailorId).Username;
                    measurementSetup.CollectionDate = assignedTailor.CollectionDate;
                }
                measurementSetup.Parameters = parameterSetups;
                setups.Add(measurementSetup);
            }
            model.Setups = setups;
            return model;
        }

        public void UpdateComputedMeasurement(List<MeasurementSetupVM> vmodel)
        {
            foreach (var measurement in vmodel)
            {
                if (measurement.ClothTypeId != 0)
                {
                    foreach (var parameter in measurement.Parameters)
                    {
                        var clothTypeMeasurement = _db.ClothTypeMeasurements.FirstOrDefault(x => x.ClothTypeID == measurement.ClothTypeId && x.MeasurementID == parameter.ParameterID);
                        var exist = _db.MeasurementCollections.FirstOrDefault(x => !x.IsDeleted && x.ClothTypeMeasurementID == clothTypeMeasurement.Id && x.BillNumber == measurement.BillNumber);
                        if (exist != null)
                        {
                            exist.Value = parameter.Value;
                            _db.Entry(exist).State = System.Data.Entity.EntityState.Modified;
                            _db.SaveChanges();
                        }
                        else
                        {
                            var measurementCollection = new MeasurementCollection()
                            {
                                ClothTypeMeasurementID = clothTypeMeasurement.Id,
                                BillNumber = measurement.BillNumber,
                                Value = parameter.Value,
                                IsDeleted = false,
                                DateCreated = DateTime.Now
                            };
                            _db.MeasurementCollections.Add(measurementCollection);
                            _db.SaveChanges();
                        }
                    }
                    foreach (var item in measurement.TailorAssignments)
                    {
                        var tailorId = _db.Users.FirstOrDefault(x => x.Username == item.Tailor).Id;
                        var checkIfAssignTailorExist = _db.AssignedTailorToBilledClothes.FirstOrDefault(x => x.BillingId == measurement.BillingID && x.TailorId == tailorId);

                        if (checkIfAssignTailorExist == null)
                        {
                            var assignTailor = new AssignedTailorToBilledCloth()
                            {
                                BillingId = measurement.BillingID,
                                DateAssigned = DateTime.Now,
                                CollectionDate = item.CollectionDate,
                                TailorId = tailorId,
                                Quantity = item.Quantity,
                            };
                            _db.AssignedTailorToBilledClothes.Add(assignTailor);
                            _db.SaveChanges();
                        }
                    }
                }
            }
        }



        //Cloth Tracking / Collection
        public List<RequestTrackerVM> TrackRequest(RequestTrackerVM vmodel)
        {
            List<RequestTrackerVM> trackedRequest = new List<RequestTrackerVM>();

            var bills = _db.Billings.Where(x => x.InvoiceNumber == vmodel.BillNumber && x.IsDeleted == false || (x.DateCreated >= vmodel.StartDate && x.DateCreated <= vmodel.EndDate)).ToList();
            foreach (var bill in bills.Distinct(x => x.InvoiceNumber))
            {
                var assignedTailorToBilledClothes = _db.AssignedTailorToBilledClothes.Where(x => x.Billing.InvoiceNumber == bill.InvoiceNumber);
                var specimenCollected = _laboratoryService.GetSpecimenCollected(bill.InvoiceNumber);
                var request = new RequestTrackerVM()
                {
                    Id = bill.Id,
                    BillNumber = bill.InvoiceNumber,
                    CustomerName = bill.CustomerName,
                    ClothCollected = bill.CollectedBy == null ? false : true,
                    ClothCollectedBy = bill.CollectedBy,
                    ClothCollectedOn = bill.DateCollected,
                    IsClothReady = assignedTailorToBilledClothes.Count() <= 0 ? false : !assignedTailorToBilledClothes.Any(o => o.IsReady == false),
                    HasCompletedPayment = _paymentService.CheckIfPaymentIsCompleted(bill.InvoiceNumber),
                };
                trackedRequest.Add(request);
            }
            return trackedRequest;
        }
        public bool SaveClothCollector(RequestTrackerVM vmodel)
        {
            bool HasSaved = false;
            var bills = _db.Billings.Where(x => x.InvoiceNumber == vmodel.BillNumber);
            foreach (var model in bills)
            {
                model.CollectedBy = vmodel.ClothCollectedBy;
                model.IssuerID = Global.AuthenticatedUserID;
                model.DateCollected = DateTime.Now;

                _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            }
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }
    }
}