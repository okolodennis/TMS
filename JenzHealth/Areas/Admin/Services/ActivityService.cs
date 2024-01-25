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

        public ActivityService()
        {
            _db = new DatabaseEntities();
        }
        public ActivityService(DatabaseEntities db)
        {
            _db = db;
            _paymentService = new PaymentService();
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
                    List<ClothStatusVM> model = _db.AssignedTailorToBilledClothes.Where(x => x.TailorId == tailor.Id).OrderByDescending(o => o.Id).Select(b => new ClothStatusVM()
                    {
                        Id = b.Id,
                        CustomerName = _db.Billings.FirstOrDefault(o => o.InvoiceNumber == b.Billing.InvoiceNumber).CustomerName,
                        CollectionDate = b.CollectionDate,
                        BillNumber = b.Billing.InvoiceNumber,
                        ClothType = b.Billing.ClothType.Name,
                        Quantity = b.Quantity,
                        Status = b.IsReady ? "Ready" : "Not Ready",
                        IsReady = b.IsReady,
                    }).ToList();
                    model.ForEach(x => x.CollectionDateString = x.CollectionDate.ToShortDateString());
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

            var billedClothTypes = GetClothesToTakeMeasurement(billNumber);

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
                var assignedTailors = _db.AssignedTailorToBilledClothes.Where(x => x.Billing.Id == measurementSetup.BillingID).Select(o => new TailorAssignment()
                {
                    Quantity = o.Quantity,
                    Tailor = o.Tailor.Username,
                    CollectionDate = o.CollectionDate,

                }).ToList();

                measurementSetup.TotalQuantity = assignedTailors.Sum(x => x.Quantity);
                measurementSetup.Parameters = parameterSetups;
                measurementSetup.TailorAssignments = assignedTailors;
                setups.Add(measurementSetup);
            }
            model.Setups = setups;
            return model;
        }

        public void UpdateComputedMeasurement(List<MeasurementSetupVM> vmodel)
        {
            if (vmodel != null)
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

                            string path = "/Content/Uploads/";
                            string styleImageFilePath = null;
                            string fabricsImageFilePath = null;
                            string targetpath = HttpContext.Current.Server.MapPath("~" + path);

                            if (measurement.StyleImageFile != null)
                            {
                                string styleImageFilename = measurement.StyleImageFile.FileName;
                                styleImageFilePath = path + styleImageFilename;
                                measurement.StyleImageFile.SaveAs(targetpath + styleImageFilename);
                            }
                            if (measurement.FabricsImageFile != null)
                            {
                                string fabricsImageFilename = measurement.FabricsImageFile.FileName;
                                fabricsImageFilePath = path + fabricsImageFilename;
                                measurement.FabricsImageFile.SaveAs(targetpath + fabricsImageFilename);
                            }
                            if (checkIfAssignTailorExist == null)
                            {
                                var assignTailor = new AssignedTailorToBilledCloth()
                                {
                                    BillingId = measurement.BillingID,
                                    DateAssigned = DateTime.Now,
                                    CollectionDate = item.CollectionDate,
                                    TailorId = tailorId,
                                    Quantity = item.Quantity,
                                    StyleImageFile = styleImageFilePath,
                                    FabricsImageFile = fabricsImageFilePath,
                                    TakenById = Global.AuthenticatedUserID,
                                };
                                _db.AssignedTailorToBilledClothes.Add(assignTailor);
                                _db.SaveChanges();
                            }
                            else
                            {
                                checkIfAssignTailorExist.Quantity = item.Quantity;
                                checkIfAssignTailorExist.CollectionDate = item.CollectionDate;
                                _db.Entry(checkIfAssignTailorExist).State = System.Data.Entity.EntityState.Modified;
                                _db.SaveChanges();

                            }
                        }
                    }
                }
            }
        }


        //Cloth Tracking / Collection
        public List<RequestTrackerVM> TrackRequest(RequestTrackerVM vmodel)
        {
            List<RequestTrackerVM> trackedRequest = new List<RequestTrackerVM>();
            List<Billing> bills = new List<Billing>();
         

            //Receipt Search
            string BillNumberFromCashCollection = "";
            if (!String.IsNullOrEmpty(vmodel.ReceiptNumber))
            {
                 BillNumberFromCashCollection = _db.CashCollections.FirstOrDefault(x => x.PaymentReciept == vmodel.ReceiptNumber).BillInvoiceNumber;
            }



            //Date
            if (vmodel.StartDate != null && vmodel.EndDate != null)
            {
                var startDate = new DateTime(vmodel.StartDate.Value.Year, vmodel.StartDate.Value.Month, vmodel.StartDate.Value.Day, DateTime.MinValue.Hour, DateTime.MinValue.Minute, DateTime.MinValue.Second);
                var endDate = new DateTime(vmodel.EndDate.Value.Year, vmodel.EndDate.Value.Month, vmodel.EndDate.Value.Day, DateTime.MaxValue.Hour, DateTime.MaxValue.Minute, DateTime.MaxValue.Second);

                //CustomerID
                if (!String.IsNullOrEmpty(vmodel.CustomerUniqueID))
                {
                    var customerId = vmodel.CustomerUniqueID.Split('|')[0].Trim();
                    bills = _db.Billings.Where(x => ((x.InvoiceNumber == vmodel.BillNumber || x.InvoiceNumber == BillNumberFromCashCollection) && x.IsDeleted == false) || (x.CustomerUniqueID == customerId && x.DateCreated >= startDate && x.DateCreated <= endDate && !x.IsDeleted)).ToList();
                }
                else
                {
                    bills = _db.Billings.Where(x => ((x.InvoiceNumber == vmodel.BillNumber || x.InvoiceNumber == BillNumberFromCashCollection) && x.IsDeleted == false) || (x.DateCreated >= startDate && x.DateCreated <= endDate && !x.IsDeleted)).ToList();
                }
            }
            else
            {
                //CustomerID
                if (!String.IsNullOrEmpty(vmodel.CustomerUniqueID))
                {
                    var customerId = vmodel.CustomerUniqueID.Split('|')[0].Trim();
                    bills = _db.Billings.Where(x => ((x.InvoiceNumber == vmodel.BillNumber || x.InvoiceNumber == BillNumberFromCashCollection) && x.IsDeleted == false) || (x.CustomerUniqueID == customerId && !x.IsDeleted)).ToList();
                }
                else
                {
                    bills = _db.Billings.Where(x => ((x.InvoiceNumber == vmodel.BillNumber || x.InvoiceNumber == BillNumberFromCashCollection) && x.IsDeleted == false)).ToList();
                }
            }

            foreach (var bill in bills.Distinct(x => x.InvoiceNumber))
            {
                var assignedTailorToBilledClothes = _db.AssignedTailorToBilledClothes.Where(x => x.Billing.InvoiceNumber == bill.InvoiceNumber);

                decimal TotalAmount = 0, TotalDeposit = 0, Balance = 0;
                if (_db.CashCollections.Any(x => x.BillInvoiceNumber == bill.InvoiceNumber))
                {
                     TotalAmount = _db.CashCollections.Where(x => x.BillInvoiceNumber == bill.InvoiceNumber && !x.IsDeleted && !x.IsCancelled).Sum(o => o.AmountPaid);
                     TotalDeposit = !_db.CashCollections.Any(x => x.BillInvoiceNumber == bill.InvoiceNumber && x.IsDeposit && !x.IsDeleted && !x.IsCancelled) ? 0 : _db.CashCollections.Where(x => x.BillInvoiceNumber == bill.InvoiceNumber && x.IsDeposit && !x.IsDeleted && !x.IsCancelled).Sum(o => o.AmountPaid);
                     Balance = (_db.CashCollections.FirstOrDefault(x => x.BillInvoiceNumber == bill.InvoiceNumber && !x.IsDeleted && !x.IsCancelled).NetAmount) - (_db.CashCollections.Where(x => x.BillInvoiceNumber == bill.InvoiceNumber && !x.IsDeleted && !x.IsCancelled).Sum(o => o.AmountPaid));
               }

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
                    TotalAmount = TotalAmount,
                    TotalDeposit = TotalDeposit,
                    Balance = Balance
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
        public ClothStatusReportVM GetClothStatusReport(ClothStatusReportVM vmodel)
        {
            List<ClothStatusReportVM> ClothReadyForCollection = new List<ClothStatusReportVM>();
            List<ClothStatusReportVM> ClothDueForCollectionButNotCollected = new List<ClothStatusReportVM>();
            List<ClothStatusReportVM> ClothCollected = new List<ClothStatusReportVM>();
            IEnumerable<AssignedTailorToBilledCloth> lists = null;

            if (vmodel.StartDate != null && vmodel.EndDate != null)
            {
                var startDate = new DateTime(vmodel.StartDate.Value.Year, vmodel.StartDate.Value.Month, vmodel.StartDate.Value.Day, DateTime.MinValue.Hour, DateTime.MinValue.Minute, DateTime.MinValue.Second);
                var endDate = new DateTime(vmodel.EndDate.Value.Year, vmodel.EndDate.Value.Month, vmodel.EndDate.Value.Day, DateTime.MaxValue.Hour, DateTime.MaxValue.Minute, DateTime.MaxValue.Second);

                lists = _db.AssignedTailorToBilledClothes.Where(x => x.DateAssigned >= startDate && x.DateAssigned <= endDate).ToList();
            }

            //ClothReadyForCollection
            if (lists != null)
            {
                ClothReadyForCollection = lists.Where(x => x.IsReady).Select(b => new ClothStatusReportVM()
                {
                    Id = b.Id,
                    CustomerName = _db.Billings.FirstOrDefault(o => o.InvoiceNumber == b.Billing.InvoiceNumber).CustomerName,
                    CustomerNumber = _db.Billings.FirstOrDefault(o => o.InvoiceNumber == b.Billing.InvoiceNumber).CustomerUniqueID,
                    Phone = _db.Billings.FirstOrDefault(o => o.InvoiceNumber == b.Billing.InvoiceNumber).CustomerPhoneNumber,
                    CollectionDate = b.CollectionDate,
                    RecievedDate = b.DateAssigned,
                    BillNumber = b.Billing.InvoiceNumber,
                    ClothType = b.Billing.ClothType.Name,
                }).ToList();
            }

            //ClothDueForCollectionButNotCollected
            if (lists != null)
            {
                ClothDueForCollectionButNotCollected = lists.Where(x => x.IsReady && x.Billing.DateCollected == null).Select(b => new ClothStatusReportVM()
                {
                    Id = b.Id,
                    CustomerName = _db.Billings.FirstOrDefault(o => o.InvoiceNumber == b.Billing.InvoiceNumber).CustomerName,
                    CustomerNumber = _db.Billings.FirstOrDefault(o => o.InvoiceNumber == b.Billing.InvoiceNumber).CustomerUniqueID,
                    Phone = _db.Billings.FirstOrDefault(o => o.InvoiceNumber == b.Billing.InvoiceNumber).CustomerPhoneNumber,
                    CollectionDate = b.CollectionDate,
                    RecievedDate = b.DateAssigned,
                    BillNumber = b.Billing.InvoiceNumber,
                    ClothType = b.Billing.ClothType.Name,
                    Tailor = b.Tailor.Username
                }).ToList();
            }

            //ClothCollected
            if (lists != null)
            {
                ClothCollected = lists.Where(x => x.IsReady && x.Billing.DateCollected != null).Select(b => new ClothStatusReportVM()
                {
                    Id = b.Id,
                    CustomerName = _db.Billings.FirstOrDefault(o => o.InvoiceNumber == b.Billing.InvoiceNumber).CustomerName,
                    CustomerNumber = _db.Billings.FirstOrDefault(o => o.InvoiceNumber == b.Billing.InvoiceNumber).CustomerUniqueID,
                    Phone = _db.Billings.FirstOrDefault(o => o.InvoiceNumber == b.Billing.InvoiceNumber).CustomerPhoneNumber,
                    CollectionDate = b.CollectionDate,
                    RecievedDate = b.DateAssigned,
                    BillNumber = b.Billing.InvoiceNumber,
                    ClothType = b.Billing.ClothType.Name,
                    ActualCollectionDate = b.Billing.DateCollected
                }).ToList();
            }
            var model = new ClothStatusReportVM(ClothReadyForCollection, ClothDueForCollectionButNotCollected, ClothCollected);
            return model;
        }
        public ClothMeasurementReportVM GetClothMeasurementReport(ClothMeasurementReportVM vmodel)
        {
            List<ClothMeasurementReportVM> ClothMeasurement = new List<ClothMeasurementReportVM>();
            IEnumerable<AssignedTailorToBilledCloth> lists = null;

            if (vmodel.StartDate != null && vmodel.EndDate != null)
            {
                var startDate = new DateTime(vmodel.StartDate.Value.Year, vmodel.StartDate.Value.Month, vmodel.StartDate.Value.Day, DateTime.MinValue.Hour, DateTime.MinValue.Minute, DateTime.MinValue.Second);
                var endDate = new DateTime(vmodel.EndDate.Value.Year, vmodel.EndDate.Value.Month, vmodel.EndDate.Value.Day, DateTime.MaxValue.Hour, DateTime.MaxValue.Minute, DateTime.MaxValue.Second);

                lists = _db.AssignedTailorToBilledClothes.Where(x => x.DateAssigned >= startDate && x.DateAssigned <= endDate).ToList();
            }
            //For ClothType
            if (vmodel.ClothTypeId != 0)
            {
                lists = lists != null ? lists.Where(x => x.Billing.ClothTypeID == vmodel.ClothTypeId) : _db.AssignedTailorToBilledClothes.Where(x => x.Billing.ClothTypeID == vmodel.ClothTypeId).ToList();
            }

            //For CustomerNumber
            if (vmodel.CustomerNumber != null)
            {
                lists = lists != null ? lists.Where(x => x.Billing.CustomerUniqueID == vmodel.CustomerNumber) : _db.AssignedTailorToBilledClothes.Where(x => x.Billing.CustomerUniqueID == vmodel.CustomerNumber).ToList();
            }
            if (lists != null)
            {
                ClothMeasurement = lists.Select(b => new ClothMeasurementReportVM()
                {
                    Id = b.Id,
                    CustomerName = _db.Billings.FirstOrDefault(o => o.InvoiceNumber == b.Billing.InvoiceNumber).CustomerName,
                    CustomerNumber = _db.Billings.FirstOrDefault(o => o.InvoiceNumber == b.Billing.InvoiceNumber).CustomerUniqueID,
                    BillNumber = b.Billing.InvoiceNumber,
                    ClothType = b.Billing.ClothType.Name,
                    Date = b.DateAssigned,
                    ClothTypeId = b.Billing.ClothType.Id,
                    //TakenBy = b.c
                }).Distinct(o => o.CustomerNumber).ToList();
            }

            var model = new ClothMeasurementReportVM(ClothMeasurement);
            model.StartDate = vmodel.StartDate;
            model.EndDate = vmodel.EndDate;
            model.ClothTypeId = vmodel.ClothTypeId;
            model.CustomerNumber = vmodel.CustomerNumber;
            return model;
        }

        public List<CustomerMeasurementVM> GetCustomerMeasurement(string CustomerNumber, int ClothTypeID, DateTime StartDate, DateTime EndDate)
        {
            //List<CustomerMeasurementVM> records = new List<CustomerMeasurementVM>();

            //if (StartDate != null && EndDate != null)
            //{
            //    var startDate = new DateTime(StartDate.Value.Year, StartDate.Value.Month, StartDate.Value.Day, DateTime.MinValue.Hour, DateTime.MinValue.Minute, DateTime.MinValue.Second);
            //    var endDate = new DateTime(EndDate.Value.Year, EndDate.Value.Month, EndDate.Value.Day, DateTime.MaxValue.Hour, DateTime.MaxValue.Minute, DateTime.MaxValue.Second);

            //    records = _db.AssignedTailorToBilledClothes.Where(x => x.DateAssigned >= startDate && x.DateAssigned <= endDate).ToList();
            //}
            ////For ClothType
            //if (ClothTypeID != 0)
            //{
            //    lists = lists != null ? lists.Where(x => x.Billing.ClothTypeID == ClothTypeID) : _db.AssignedTailorToBilledClothes.Where(x => x.Billing.ClothTypeID == ClothTypeId).ToList();
            //}

            ////For CustomerNumber
            //if (CustomerNumber != null)
            //{
            //    lists = lists != null ? lists.Where(x => x.Billing.CustomerUniqueID == CustomerNumber) : _db.AssignedTailorToBilledClothes.Where(x => x.Billing.CustomerUniqueID == CustomerNumber).ToList();
            //}
            //var record = _db.MeasurementCollections.Where(x => !x.IsDeleted && x.DateCreated >= StartDate && x.DateCreated <= EndDate)
            //    .Join(_db.Billings.Where(x => x.CustomerUniqueID == CustomerNumber && !x.IsDeleted && x.DateCreated >= StartDate && x.DateCreated <= EndDate)
            //    meas => meas.BillNumber, bill => bill.InvoiceNumber); (meas, bill) => new CustomerMeasurementVM()
            //    //{
                //    CustomerName = bill.CustomerName,
                //    CustomerNumber = bill.CustomerUniqueID,
                //    ClothType = meas.ClothTypeMeasurement.ClothType.Name,
                //    Parameter = meas.ClothTypeMeasurement.Measurement.Name,
                //    Value = meas.Value,
                //    DateCreated = meas.DateCreated
                //}).ToList<CustomerMeasurementVM>();

            var record = _db.MeasurementCollections.Where(x => !x.IsDeleted && x.DateCreated >= StartDate && x.DateCreated <= EndDate)
                .Join(_db.Billings.Where(x => x.CustomerUniqueID == CustomerNumber && !x.IsDeleted && x.DateCreated >= StartDate && x.DateCreated <= EndDate),
                meas => meas.BillNumber, bill => bill.InvoiceNumber, (meas, bill) => new CustomerMeasurementVM()
                {
                    CustomerName = bill.CustomerName,
                    CustomerNumber = bill.CustomerUniqueID,
                    ClothType = meas.ClothTypeMeasurement.ClothType.Name,
                    Parameter = meas.ClothTypeMeasurement.Measurement.Name,
                    Value = meas.Value,
                    DateCreated = meas.DateCreated
                }).ToList<CustomerMeasurementVM>();

            record.ForEach(x => x.Date = x.DateCreated.ToShortDateString());
            return record;
            //_db.MeasurementCollections.Where(x => x.ClothTypeMeasurement.Measurement.ClothTypeID == ClothTypeId && x.BillNumber == BillNumber && x.IsDeleted == false).Select
            //   (p=> new CustomerMeasurementVM() { 
            //       ClothType= p.ClothTypeMeasurement.ClothType.Name,
            //       Parameter = p.ClothTypeMeasurement.Measurement.Name,
            //       Value = p.Value,
            //       CustomerName = _db.Billings.FirstOrDefault(o => o.InvoiceNumber == p.BillNumber).CustomerName,
            //       CustomerNumber = _db.Billings.FirstOrDefault(o => o.InvoiceNumber == p.BillNumber).CustomerUniqueID,
            //   }).ToList();
        }
    }
}