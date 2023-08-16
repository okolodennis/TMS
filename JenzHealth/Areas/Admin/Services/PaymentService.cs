using WebApp.Areas.Admin.Helpers;
using WebApp.Areas.Admin.Interfaces;
using WebApp.Areas.Admin.ViewModels;
using WebApp.Areas.Admin.ViewModels.Report;
using WebApp.DAL.DataConnection;
using WebApp.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.Services
{
    public class PaymentService : IPaymentService
    {
        readonly DatabaseEntities _db;
        IUserService _userService;
        public PaymentService()
        {
            _db = new DatabaseEntities();
            _userService = new UserService(_db);
        }
        public PaymentService(DatabaseEntities db, UserService userService)
        {
            _db = db;
            _userService = userService;
        }
        public string CreateBilling(BillingVM vmodel, List<ServiceListVM> serviceList)
        {
            var billCount = _db.ApplicationSettings.FirstOrDefault().BillCount;
            billCount++;
            var invoiceNumber = string.Format("BN{0}", billCount.ToString("D6"));

            foreach (var service in serviceList)
            {
                var model = new Billing()
                {
                    CustomerType = vmodel.CustomerType,
                    CustomerUniqueID = vmodel.CustomerType == CustomerType.REGISTERED_CUSTOMER ? vmodel.CustomerUniqueID : null,
                    CustomerName = vmodel.CustomerName,
                    CustomerGender = vmodel.CustomerGender,
                    CustomerAge = vmodel.CustomerAge,
                    CustomerPhoneNumber = vmodel.CustomerPhoneNumber,
                    IsDeleted = false,
                    InvoiceNumber = invoiceNumber,
                    DateCreated = DateTime.Now,
                    // ServiceID = service.ServiceID,
                    ClothTypeID = service.ServiceID,
                    GrossAmount = service.GrossAmount,
                    Quantity = service.Quantity,
                    BilledByID = Global.AuthenticatedUserID
                };
                _db.Billings.Add(model);
            }

            var updateSettings = _db.ApplicationSettings.FirstOrDefault();
            updateSettings.BillCount = billCount;
            _db.Entry(updateSettings).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();

            return invoiceNumber;
        }
        public string UpdateBilling(BillingVM vmodel, List<ServiceListVM> serviceList)
        {
            var ServiceBills = _db.Billings.Where(x => x.InvoiceNumber == vmodel.InvoiceNumber && x.IsDeleted == false).ToList();
            if (ServiceBills.Count > 0)
            {
                foreach (var service in ServiceBills)
                {
                    service.IsDeleted = true;
                    _db.Entry(service).State = System.Data.Entity.EntityState.Modified;
                    _db.SaveChanges();
                }
            }
            if (serviceList.Count > 0)
            {
                foreach (var service in serviceList)
                {
                    var model = new Billing()
                    {
                        CustomerType = vmodel.CustomerType,
                        CustomerUniqueID = vmodel.CustomerType == CustomerType.REGISTERED_CUSTOMER ? _db.Billings.FirstOrDefault(x => x.InvoiceNumber == vmodel.InvoiceNumber).CustomerUniqueID : null,
                        CustomerName = vmodel.CustomerName,
                        CustomerGender = vmodel.CustomerGender,
                        CustomerAge = vmodel.CustomerAge,
                        CustomerPhoneNumber = vmodel.CustomerPhoneNumber,
                        IsDeleted = false,
                        InvoiceNumber = vmodel.InvoiceNumber,
                        DateCreated = DateTime.Now,
                        ClothTypeID = service.ServiceID,
                        // ServiceID = service.ServiceID,
                        GrossAmount = service.GrossAmount,
                        Quantity = service.Quantity,
                        BilledByID = _userService.GetCurrentUser().Id
                    };
                    _db.Billings.Add(model);
                }
            }
            _db.SaveChanges();

            return vmodel.InvoiceNumber;
        }
        public BillingVM GetCustomerForBill(string invoiceNumber)
        {
            var model = _db.Billings.Where(x => x.InvoiceNumber == invoiceNumber && x.IsDeleted == false).Select(b => new BillingVM()
            {
                CustomerName = b.CustomerName == null ? _db.Customers.FirstOrDefault(x => x.CustomerUniqueID == b.CustomerUniqueID).Firstname + " " + _db.Customers.FirstOrDefault(x => x.CustomerUniqueID == b.CustomerUniqueID).Lastname : b.CustomerName,
                CustomerGender = b.CustomerGender == null ? _db.Customers.FirstOrDefault(x => x.CustomerUniqueID == b.CustomerUniqueID).Gender : b.CustomerGender,
                CustomerPhoneNumber = b.CustomerPhoneNumber == null ? _db.Customers.FirstOrDefault(x => x.CustomerUniqueID == b.CustomerUniqueID).PhoneNumber : b.CustomerPhoneNumber,
              //  CustomerAge = b.CustomerAge == 0 ? DateTime.Now.Year - _db.Customers.FirstOrDefault(x => x.CustomerUniqueID == b.CustomerUniqueID).DOB.Year : b.CustomerAge,
                InvoiceNumber = b.InvoiceNumber
            }).FirstOrDefault();
            return model;
        }
        public List<BillingVM> GetCustomerForReport(string invoiceNumber)
        {
            var model = _db.Billings.Where(x => x.InvoiceNumber == invoiceNumber && x.IsDeleted == false).Select(b => new BillingVM()
            {
                CustomerName = b.CustomerName == null ? _db.Customers.FirstOrDefault(x => x.CustomerUniqueID == b.CustomerUniqueID).Firstname + " " + _db.Customers.FirstOrDefault(x => x.CustomerUniqueID == b.CustomerUniqueID).Lastname : b.CustomerName,
                CustomerGender = b.CustomerGender == null ? _db.Customers.FirstOrDefault(x => x.CustomerUniqueID == b.CustomerUniqueID).Gender : b.CustomerGender,
                CustomerPhoneNumber = b.CustomerPhoneNumber == null ? _db.Customers.FirstOrDefault(x => x.CustomerUniqueID == b.CustomerUniqueID).PhoneNumber : b.CustomerPhoneNumber,
               // CustomerAge = b.CustomerAge == 0 ? DateTime.Now.Year - _db.Customers.FirstOrDefault(x => x.CustomerUniqueID == b.CustomerUniqueID).DOB.Year : b.CustomerAge,
                CustomerUniqueID = b.CustomerUniqueID,
                InvoiceNumber = b.InvoiceNumber.ToUpper()
            }).ToList();
            return model;
        }
        public List<BillingVM> GetBillServices(string invoiceNumber)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            var model = _db.Billings.Where(x => x.InvoiceNumber == invoiceNumber && x.IsDeleted == false).Select(b => new BillingVM()
            {
                Id = b.ClothTypeID,
                ServiceName = b.ClothType.Name,
                Quantity = b.Quantity,
                SellingPrice = b.ClothType.CostPrice,
            }).ToList();
            foreach (var each in model)
            {
                each.GrossAmount = (each.Quantity * each.SellingPrice);
                each.SellingPriceString = "₦" + each.SellingPrice.ToString("N", nfi);
            }
            return model;
        }

        public List<BillDetailsVM> GetBillingDetails(string billnumber)
        {
            var billList = new List<BillDetailsVM>();

            var bill = _db.Billings.Where(x => x.IsDeleted == false && x.InvoiceNumber == billnumber).Select(b => new BillDetailsVM()
            {
                BilledBy = b.BilledBy.Firstname + " " + b.BilledBy.Lastname,
                BillInvoiceNumber = b.InvoiceNumber.ToUpper(),
            }).FirstOrDefault();
            var waived = GetWaivedAmountForBillInvoiceNumber(billnumber);
            decimal waivedAmount = 0;
            if (waived != null)
                waivedAmount = waived.WaiveAmount;

            bill.WaivedAmount = waivedAmount;
            bill.NetAmount = GetBillServices(billnumber).Sum(x => x.GrossAmount);
            bill.BalanceAmount = (bill.NetAmount - bill.WaivedAmount);

            billList.Add(bill);

            return billList;
        }

        public bool WaiveAmountForCustomer(WaiverVM vmodel)
        {
            var checkIfWaivedBefore = _db.Waivers.Count(x => x.BillInvoiceNumber == vmodel.BillInvoiceNumber && x.IsDeleted == false);
            if (checkIfWaivedBefore > 0)
            {
                var model = _db.Waivers.FirstOrDefault(x => x.BillInvoiceNumber == vmodel.BillInvoiceNumber && x.IsDeleted == false);
                model.AvailableAmount = vmodel.AvailableAmount;
                model.WaiveAmount = vmodel.WaiveAmount;
                model.NetAmount = vmodel.NetAmount;
                model.WaiveBy = vmodel.WaiveBy;
                model.WaivedByID = _userService.GetCurrentUser().Id;

                _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                var model = new Waiver()
                {
                    BillInvoiceNumber = vmodel.BillInvoiceNumber,
                    AvailableAmount = vmodel.AvailableAmount,
                    NetAmount = vmodel.NetAmount,
                    WaiveAmount = vmodel.WaiveAmount,
                    WaiveBy = vmodel.WaiveBy,
                    IsDeleted = false,
                    DateCreated = DateTime.Now,
                    WaivedByID = _userService.GetCurrentUser().Id
                };
                _db.Waivers.Add(model);
            }
            _db.SaveChanges();

            return true;
        }
        public Waiver GetWaivedAmountForBillInvoiceNumber(string billInvoiceNumber)
        {
            return _db.Waivers.FirstOrDefault(x => x.BillInvoiceNumber == billInvoiceNumber && x.IsDeleted == false);
        }
        public List<PartPaymentVM> GetPartPayments(string BillInvoiceNumber)
        {
            var model = _db.PartPayments.Where(x => x.BillInvoiceNumber == BillInvoiceNumber && x.IsDeleted == false).Select(b => new PartPaymentVM()
            {
                Id = b.Id,
                InstallmentName = b.InstallmentName,
                PartPaymentAmount = b.PartPaymentAmount,
                IsPaidPartPayment = b.IsPaidPartPayment,
            }).ToList();
            foreach (var partpayment in model)
            {
                partpayment.HasPaid = this.HasPaidInstallment(partpayment.Id);
            }
            return model;
        }
        public bool HasPaidInstallment(int installmentID)
        {
            var paidinstallment = _db.CashCollections.Count(x => x.PartPaymentID == installmentID);
            if (paidinstallment > 0)
                return true;
            else
                return false;
        }
        public bool MapPartPayment(List<PartPaymentVM> vmodel)
        {
            if (vmodel != null)
            {
                var billInvoiceNumber = vmodel.FirstOrDefault().BillInvoiceNumber;
                // Update installment
                var installments = _db.PartPayments.Where(x => x.BillInvoiceNumber == billInvoiceNumber && x.IsDeleted == false).ToList();
                foreach (var installment in installments)
                {
                    installment.IsDeleted = true;
                    _db.Entry(installment).State = System.Data.Entity.EntityState.Modified;
                }
                _db.SaveChanges();

                foreach (var installment in vmodel)
                {
                    // Add installment
                    var model = new PartPayment()
                    {
                        BillInvoiceNumber = installment.BillInvoiceNumber,
                        InstallmentName = installment.InstallmentName,
                        PartPaymentAmount = installment.PartPaymentAmount,
                        IsPaidPartPayment = false,
                        IsDeleted = false,
                        DateCreated = DateTime.Now,
                        CreatedByID = _userService.GetCurrentUser().Id
                    };
                    _db.PartPayments.Add(model);
                }
                _db.SaveChanges();
            }
            return true;
        }
        public bool Deposite(DepositeCollectionVM vmodel)
        {
            var depositeCount = _db.ApplicationSettings.FirstOrDefault().DepositeCount;
            depositeCount++;
            var model = new DepositeCollection()
            {
                BillInvoiceNumber =  vmodel.BillInvoiceNumber,
                Amount = CustomSerializer.UnMaskString(vmodel.AmountString),
                CustomerUniqueID = vmodel.CustomerUniqueID,
                CustomerID = _db.Customers.FirstOrDefault(x => x.CustomerUniqueID == vmodel.CustomerUniqueID).Id,
                Description = vmodel.Description,
                PaymentType = vmodel.PaymentType,
                ReferenceNumber = vmodel.ReferenceNumber,
                DepositeReciept = String.Format("DP{0}", depositeCount.ToString("D6")),
                IsDeleted = false,
                DateCreated = DateTime.Now,
                DepositedByID = _userService.GetCurrentUser().Id
            };
            _db.DepositeCollections.Add(model);

            var updatesettings = _db.ApplicationSettings.FirstOrDefault();
            updatesettings.DepositeCount = depositeCount;

            _db.Entry(updatesettings).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();

            return true;
        }
        public CashCollectionVM CashCollection(CashCollectionVM vmodel, List<ServiceListVM> serviceList)
        {
            var shift = _userService.GetShift();
            var paymentCount = _db.ApplicationSettings.FirstOrDefault().PaymentCount;
            paymentCount++;
            string billInvoiceNumber = string.Empty;
            switch (vmodel.CollectionType)
            {
                case CollectionType.BILLED:

                    var billCashCollection = new CashCollection()
                    {
                        BillInvoiceNumber = vmodel.BillInvoiceNumber,
                        AmountPaid = vmodel.AmountToDeposit != 0 ? vmodel.AmountToDeposit : vmodel.PartPaymentID == null ? vmodel.AmountPaid : _db.PartPayments.FirstOrDefault(x => x.Id == vmodel.PartPaymentID).PartPaymentAmount,
                      //  AmountPaid = vmodel.AmountToDeposit != 0 ? vmodel.AmountToDeposit : vmodel.PartPaymentID == null ? (vmodel.NetAmount - vmodel.WaivedAmount) : _db.PartPayments.FirstOrDefault(x => x.Id == vmodel.PartPaymentID).PartPaymentAmount,
                        NetAmount = vmodel.NetAmount,
                        DatePaid = DateTime.Now,
                        WaivedAmount = vmodel.WaivedAmount,
                        BalanceAmount = vmodel.NetAmount - vmodel.WaivedAmount,
                        IsDeleted = false,
                        TransactionReferenceNumber = vmodel.TransactionReferenceNumber,
                        InstallmentType = vmodel.AmountToDeposit != 0 ? "DEPOSIT" : vmodel.PartPaymentID == null ? "FULL" : "PART",
                        PartPaymentID = vmodel.PartPaymentID,
                        PaymentType = vmodel.PaymentType,
                        ShiftID = shift.Id,
                        PaymentReciept = String.Format("PR{0}", paymentCount.ToString("D6")),
                        CollectedByID = _userService.GetCurrentUser().Id,
                        IsDeposit = vmodel.AmountToDeposit != 0 ? true : false
                    };
                    _db.CashCollections.Add(billCashCollection);
                    vmodel.PaymentReciept = billCashCollection.PaymentReciept;
                    billInvoiceNumber = vmodel.BillInvoiceNumber;
                    break;
                case CollectionType.UNBILLED:
                    // Create bill for registered customer
                    var customer = _db.Customers.FirstOrDefault(x => x.CustomerUniqueID == vmodel.CustomerUniqueID && x.IsDeleted == false);
                    var registeredCustomerBill = new BillingVM()
                    {
                        CustomerUniqueID = vmodel.CustomerUniqueID,
                        CustomerName = string.Format("{0} {1}", customer.Firstname, customer.Lastname),
                        CustomerGender = customer.Gender,
                       // CustomerAge = (DateTime.Now.Year - customer.DOB.Year),
                        CustomerPhoneNumber = customer.PhoneNumber,
                        CustomerType = CustomerType.REGISTERED_CUSTOMER,
                        CustomerID = customer.Id,
                    };
                    if (serviceList.Count > 0)
                    {
                        billInvoiceNumber = this.CreateBilling(registeredCustomerBill, serviceList);

                        var unbilledCashCollection = new CashCollection()
                        {
                            BillInvoiceNumber = billInvoiceNumber,
                            TransactionReferenceNumber = vmodel.TransactionReferenceNumber,
                            DatePaid = DateTime.Now,
                            IsDeleted = false,
                            AmountPaid = vmodel.AmountPaid,
                            WaivedAmount = vmodel.WaivedAmount,
                            BalanceAmount = vmodel.NetAmount - vmodel.WaivedAmount,
                            NetAmount = vmodel.NetAmount,
                            PaymentType = vmodel.PaymentType,
                            PartPaymentID = vmodel.PartPaymentID,
                            InstallmentType = "FULL",
                            ShiftID = shift.Id,
                            PaymentReciept = String.Format("PR{0}", paymentCount.ToString("D6")),
                            CollectedByID = _userService.GetCurrentUser().Id
                        };

                        _db.CashCollections.Add(unbilledCashCollection);
                        vmodel.PaymentReciept = unbilledCashCollection.PaymentReciept;

                    }
                    break;
                //case CollectionType.WALK_IN:
                //    // Create bill for walk-in customer
                //    var walkinBill = new BillingVM()
                //    {
                //        CustomerName = vmodel.CustomerName,
                //        CustomerGender = vmodel.CustomerGender,
                //        CustomerAge = vmodel.CustomerAge,
                //        CustomerPhoneNumber = vmodel.CustomerPhoneNumber,
                //        CustomerType = CustomerType.WALK_IN_CUSTOMER,
                //    };
                //    if (serviceList.Count > 0)
                //    {
                //        billInvoiceNumber = this.CreateBilling(walkinBill, serviceList);

                //        var walkinCashCollection = new CashCollection()
                //        {
                //            BillInvoiceNumber = billInvoiceNumber,
                //            TransactionReferenceNumber = vmodel.TransactionReferenceNumber,
                //            DatePaid = DateTime.Now,
                //            IsDeleted = false,
                //            AmountPaid = vmodel.AmountPaid,
                //            NetAmount = vmodel.NetAmount,
                //            PaymentType = vmodel.PaymentType,
                //            BalanceAmount = vmodel.NetAmount - vmodel.WaivedAmount,
                //            PartPaymentID = vmodel.PartPaymentID,
                //            InstallmentType = "FULL",
                //            ShiftID = shift.Id,
                //            PaymentReciept = String.Format("PR{0}", paymentCount.ToString("D6")),
                //            CollectedByID = _userService.GetCurrentUser().Id
                //        };

                //        _db.CashCollections.Add(walkinCashCollection);
                //        vmodel.PaymentReciept = walkinCashCollection.PaymentReciept;
                //    }
                //    break;
            }
            var updatesettings = _db.ApplicationSettings.FirstOrDefault();
            updatesettings.PaymentCount = paymentCount;
            vmodel.BillInvoiceNumber = billInvoiceNumber;
            _db.Entry(updatesettings).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();

            if (updatesettings.ExpressWaiver)
            {
                var waiver = new WaiverVM()
                {
                    BillInvoiceNumber = billInvoiceNumber,
                    AvailableAmount = vmodel.BalanceAmount,
                    NetAmount = vmodel.NetAmount,
                    WaiveAmount = vmodel.WaivedAmount,
                    WaiveBy = vmodel.WaiveBy,
                };

                this.WaiveAmountForCustomer(waiver);
            }

            return vmodel;
        }
        public List<CashCollectionVM> GetPaymentDetails(string recieptnumber)
        {
            var payments = _db.CashCollections.Where(x => x.IsDeleted == false && x.PaymentReciept == recieptnumber).Select(b => new CashCollectionVM()
            {
                Id = b.Id,
                PaymentReciept = b.PaymentReciept.ToUpper(),
                BillInvoiceNumber = b.BillInvoiceNumber.ToUpper(),
                AmountPaid = b.AmountPaid,
                DatePaid = b.DatePaid,
                TransactionReferenceNumber = b.TransactionReferenceNumber == null ? "NIL" : b.TransactionReferenceNumber.ToUpper(),
                PaymentType = b.PaymentType,
                InstallmentType = b.IsDeposit == true ? "DEPOSIT" : b.InstallmentType.ToUpper(),
                PartPayment = b.IsDeposit == true ? "DEPOSIT" : b.PartPayment == null ? "FULL PAYMENT" : b.PartPayment.InstallmentName,
                CollectedBy = b.CollectedBy.Firstname + " " + b.CollectedBy.Lastname,
                ShiftNumber = b.Shift.ShiftUniqueID
            }).ToList();
            foreach (var payment in payments)
            {
                payment.PaymentTypee = payment.PaymentType.DisplayName();
                var waived = GetWaivedAmountForBillInvoiceNumber(payment.BillInvoiceNumber);
                decimal waivedAmount = 0;
                decimal totalPaidBills = this.GetTotalPaidBillAmount(payment.BillInvoiceNumber);
                if (waived != null)
                    waivedAmount = waived.WaiveAmount;

                payment.WaivedAmount = waivedAmount;
                payment.TotalAmountPaid = totalPaidBills;
                payment.NetAmount = GetBillServices(payment.BillInvoiceNumber).Sum(x => x.GrossAmount);
                payment.BalanceAmount = (payment.NetAmount - (payment.WaivedAmount + totalPaidBills));
            }
            return payments;
        }
        public TransactionVM GetTransactionReports(TransactionVM vmodel)
        {
            var model = new TransactionVM();
            var summaries = _db.Shifts.Where(x => x.User.Username == vmodel.ShiftUniqeID || x.ShiftUniqueID == vmodel.ShiftUniqeID || (x.DateTimeCreated >= vmodel.ShiftStarts && x.ExpiresDateTime <= vmodel.ShiftEnds)).Select(b => new TransactionVM()
            {
                Id = b.Id,
                ShiftUniqeID = b.ShiftUniqueID,
                Username = b.User.Username,
                Name = b.User.Firstname + " " + b.User.Lastname,
                PhoneNumber = b.User.PhoneNumber,
                Email = b.User.Email,
                ShiftStarts = b.DateTimeCreated,
                ShiftEnds = b.ExpiresDateTime,
                ShiftStatus = b.HasExpired ? "Closed" : "Open",
                ShiftClosedBy = b.HasExpired ? b.ClosedBy : " - ",
                TransactionCount = _db.CashCollections.Count(x => x.Shift.Id == b.Id),
                TotalAmount = _db.CashCollections.Count(x => x.ShiftID == b.Id) == 0 ? 0 : _db.CashCollections.Where(x => x.ShiftID == b.Id).ToList().Sum(x => x.AmountPaid) ,
            }).OrderByDescending(x => x.Id).ToList();

            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

            foreach (var summary in summaries)
            {
                summary.TotalAmountString = "₦" + summary.TotalAmount.ToString("N", nfi);
            }

            model.TableData = summaries;

            return model;
        }
        public TransactionVM GetShiftTransactionDetails(int shiftID)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            var model = new TransactionVM();
            var shift = _db.Shifts.Where(x => x.Id == shiftID).Select(b => new TransactionVM()
            {
                Id = b.Id,
                ShiftUniqeID = b.ShiftUniqueID,
                Username = b.User.Username,
                Name = b.User.Firstname + " " + b.User.Lastname,
                PhoneNumber = b.User.PhoneNumber,
                Email = b.User.Email,
                ShiftStarts = b.DateTimeCreated,
                ShiftEnds = b.ExpiresDateTime,
                ShiftStatus = b.HasExpired ? "Closed" : "Open",
                ShiftClosedBy = b.HasExpired ? b.ClosedBy : " - ",
                TransactionCount = _db.CashCollections.Count(x => x.Shift.Id == b.Id),
                TotalAmount = _db.CashCollections.Where(x => x.ShiftID == b.Id).ToList().Sum(x => x.AmountPaid),
            }).FirstOrDefault();
            shift.CashAmountString = this.CalcualateTotalAmount(shiftID, PaymentType.CASH);
            shift.POSAmountString = this.CalcualateTotalAmount(shiftID, PaymentType.POS);
            shift.EFTAmountString = this.CalcualateTotalAmount(shiftID, PaymentType.EFT);
            shift.TotalAmountString = "₦" + shift.TotalAmount.ToString("N", nfi);

            return shift;
        }
        public SharedRevenueReportVM GetSharedRevenueReport(SharedRevenueReportVM vmodel)
        {

            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            var model = new SharedRevenueReportVM();
            List<SharedRevenueReportVM> records = new List<SharedRevenueReportVM>();
            List<string> cashCollections = new List<string>();

            if (vmodel.StartDate != null && vmodel.EndDate != null)
            {
                var startDate = new DateTime(vmodel.StartDate.Value.Year, vmodel.StartDate.Value.Month, vmodel.StartDate.Value.Day, DateTime.MinValue.Hour, DateTime.MinValue.Minute, DateTime.MinValue.Second);
                var endDate = new DateTime(vmodel.EndDate.Value.Year, vmodel.EndDate.Value.Month, vmodel.EndDate.Value.Day, DateTime.MaxValue.Hour, DateTime.MaxValue.Minute, DateTime.MaxValue.Second);

                model.StartDateString = startDate.ToShortDateString();
                model.EndDateString = endDate.ToShortDateString();
                cashCollections = _db.CashCollections.Where(x => x.DatePaid >= startDate && x.DatePaid <= endDate && !x.IsDeposit && !x.IsDeleted && !x.IsCancelled).Select(o => o.BillInvoiceNumber).ToList();
            }
            else
            {
                cashCollections = _db.CashCollections.Where(x => !x.IsDeposit && !x.IsDeleted && !x.IsCancelled).Select(o => o.BillInvoiceNumber).ToList();
            }

            var Tailor = _db.Users.FirstOrDefault(x => x.Username == vmodel.Tailor && !x.IsDeleted);
            if (Tailor != null)
            {
                model.TailorName = Tailor?.Lastname + " " + Tailor?.Firstname;
                var getAssignedTailor = _db.AssignedTailorToBilledClothes.Where(x => x.IsReady && x.TailorId == Tailor.Id).Select(o => o.Billing.InvoiceNumber).ToList();
                var transactions = getAssignedTailor.Intersect(cashCollections);
                //cashCollections.Join(_db.AssignedTailorToBilledClothes.Where(x=>x.IsReady ), cash => cash.BillInvoiceNumber, asstailor => asstailor.Billing.InvoiceNumber, (cash, asstailor) => new );

                //var transactions = _db.AssignedTailorToBilledClothes.Where(x =>  x.TailorId == Tailor.Id)
                //    .Join(_db.CashCollections.Where(x => !x.IsDeleted && !x.IsCancelled),
                //    asstailor => asstailor.Billing.InvoiceNumber, cash => cash.BillInvoiceNumber, (asstailor, cash) => new 
                //    {
                //        asstailor.BillingId,
                //        asstailor.Billing.ClothTypeID,
                //        cash.BillInvoiceNumber,
                //        asstailor.TailorId,
                //        cash.BalanceAmount,
                //        cash.DatePaid,
                //        cash.PaymentReciept,
                //        cash.IsDeposit
                //    });

                foreach (var item in transactions)
                {
                    var billings = _db.Billings.Where(x => x.InvoiceNumber == item && x.IsDeleted == false).ToList();
                    foreach (var billing in billings)
                    {
                        var clothTypeId = billing.ClothTypeID;
                        var getSettlement = _db.SettlementSetups.FirstOrDefault(x => x.TailorID == Tailor.Id && x.ClothTypeID == clothTypeId && x.IsActive && !x.IsDeleted);
                        var tailorAssignment = _db.AssignedTailorToBilledClothes.FirstOrDefault(x => x.BillingId == billing.Id && x.IsReady);
                        var Quantity = tailorAssignment.Quantity;

                        if (getSettlement != null)
                        {
                            var BillInvoiceNumber = billing.InvoiceNumber;
                            var clothType = _db.ClothTypes.FirstOrDefault(x => x.Id == clothTypeId);
                            var TotalCharge = (clothType.CostPrice * Quantity);
                            var partnerShare = (TotalCharge * (getSettlement.PartnerPercent / 100));
                            var record = new SharedRevenueReportVM()
                            {
                                Quantity = Quantity,
                                BillNumber = BillInvoiceNumber,
                                ClothType = clothType.Name,
                                UnitCharge = clothType.CostPrice,
                                TotalCharge = TotalCharge,
                                FinalReceiptNo = _db.CashCollections.FirstOrDefault(x => x.BillInvoiceNumber == billing.InvoiceNumber && !x.IsDeposit && !x.IsDeleted && !x.IsCancelled)?.PaymentReciept,
                                Date = _db.CashCollections.FirstOrDefault(x => x.BillInvoiceNumber == billing.InvoiceNumber && !x.IsDeposit && !x.IsDeleted && !x.IsCancelled).DatePaid,
                                PartnerShare = partnerShare,
                                OwnerShare = (TotalCharge - partnerShare),
                            };

                            records.Add(record);
                        }
                    }
                }
                model.TableData = records;
                model.TotalUnitCharge = records.Sum(x=>x.UnitCharge);
                model.TotalOwnerShare = records.Sum(x=>x.OwnerShare);
                model.TotalPartnerShare = records.Sum(x=>x.PartnerShare);
                model.TotalTotalCharge = records.Sum(x=>x.TotalCharge);
                model.TotalQuantity = records.Sum(x=>x.Quantity);

            }
            return model;
        }
        public List<CashCollectionVM> GetCashCollectionsForShift(int shiftID)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            var cashcollections = _db.CashCollections.Where(x => x.ShiftID == shiftID).Select(b => new CashCollectionVM()
            {
                Id = b.Id,
                AmountPaid = b.AmountPaid,
                PaymentReciept = b.PaymentReciept,
                InstallmentType = b.InstallmentType,
                TransactionReferenceNumber = b.TransactionReferenceNumber == null ? " - " : b.TransactionReferenceNumber,
                PaymentType = (PaymentType)b.PaymentType,
                BillInvoiceNumber = b.BillInvoiceNumber,
                CustomerName = _db.Billings.FirstOrDefault(x => x.InvoiceNumber == b.BillInvoiceNumber).CustomerName,
                CustomerGender = _db.Billings.FirstOrDefault(x => x.InvoiceNumber == b.BillInvoiceNumber).CustomerGender,
                CustomerUniqueID = _db.Billings.FirstOrDefault(x => x.InvoiceNumber == b.BillInvoiceNumber).CustomerUniqueID == null ? " - " : _db.Billings.FirstOrDefault(x => x.InvoiceNumber == b.BillInvoiceNumber).CustomerUniqueID,
                CustomerPhoneNumber = _db.Billings.FirstOrDefault(x => x.InvoiceNumber == b.BillInvoiceNumber).CustomerPhoneNumber,
                CustomerType = ((CustomerType)_db.Billings.FirstOrDefault(x => x.InvoiceNumber == b.BillInvoiceNumber).CustomerType).ToString(),
            }).ToList();
            foreach (var cashcollection in cashcollections)
            {
                cashcollection.AmountPaidString = "₦" + cashcollection.AmountPaid.ToString("N", nfi);
            }
            return cashcollections;
        }
        public string CalcualateTotalAmount(int shiftID, PaymentType paymentType)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            decimal total = 0;
            var cashcollections = _db.CashCollections.Where(x => x.ShiftID == shiftID && x.PaymentType == paymentType).ToList();
            if (cashcollections.Count > 0)
            {
                total = cashcollections.Sum(x => x.AmountPaid);
            }
            else
            {
                total = 0;
            }
            return "₦" + total.ToString("N", nfi);
        }

        public string GetBillNumberWithReceipt(string receipt)
        {
            var model = _db.CashCollections.Where(x => x.PaymentReciept == receipt).Select(b => b.BillInvoiceNumber).FirstOrDefault();
            return model;
        }
        public bool Refund(RefundVM vmodel)
        {
            var model = new Refund()
            {
                PaymentReciept = vmodel.PaymentReciept,
                DepositeReciept = vmodel.DepositeReciept,
                AmountToRefund = vmodel.AmountToRefund,
                Comment = vmodel.Comment,
                DateCreated = DateTime.Now,
                IsDeletetd = false
            };
            _db.Refunds.Add(model);
            _db.SaveChanges();

            return true;
        }
        public void CancelReciept(CashCollectionVM vmodel)
        {
            var cashcollection = _db.CashCollections.FirstOrDefault(x => x.PaymentReciept == vmodel.PaymentReciept);
            cashcollection.IsCancelled = true;
            cashcollection.Comment = vmodel.Comment;
            cashcollection.CanceledByID = _userService.GetCurrentUser().Id;

            _db.Entry(cashcollection).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
        }
        public decimal GetTotalPaidBillAmount(string billnumber)
        {
            decimal totalbillamount = 0;
            var billamount = _db.CashCollections.Where(x => x.BillInvoiceNumber == billnumber);
            if (billamount.Count() > 0)
            {
                totalbillamount = billamount.Sum(b => b.AmountPaid);
            }
            else
            {
                totalbillamount = 0;
            }

            return totalbillamount;
        }

        public bool CheckIfPaymentIsCompleted(string billnumber)
        {
            var totalAmountToPay = GetBillingDetails(billnumber).FirstOrDefault().BalanceAmount;
            var totalAmountPaid = GetTotalPaidBillAmount(billnumber);

            if (totalAmountPaid != totalAmountToPay)
                return false;
            else
                return true;
        }
    }

}