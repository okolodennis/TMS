using WebApp.Areas.Admin.Interfaces;
using WebApp.Areas.Admin.ViewModels;
using WebApp.DAL.DataConnection;
using WebApp.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace WebApp.Areas.Admin.Services
{
    public class CustomerService : ICustomerService
    {

        /* Instancation of the database context model
      * and injecting some buisness layer services
      */
        #region Instanciation
        readonly DatabaseEntities _db;
        readonly IPaymentService _paymentService;
        public CustomerService()
        {
            _db = new DatabaseEntities();
            _paymentService = new PaymentService();
        }
        public CustomerService(DatabaseEntities db)
        {
            _db = db;
            _paymentService = new PaymentService();
        }
        #endregion

        /* *************************************************************************** */
        //Customer

        // Fetching Customer
        public List<CustomerVM> GetCustomers()
        {
            var model = _db.Customers.Where(x => x.IsDeleted == false).Select(b => new CustomerVM()
            {
                Id = b.Id,
                Firstname = b.Firstname,
                Lastname = b.Lastname,
                CustomerUniqueID = b.CustomerUniqueID,
                Gender = b.Gender,
                DOB = DateTime.Now,
               // Religion = b.Religion,
                Address = b.Address,
                PhoneNumber = b.PhoneNumber,
                Email = b.Email
            }).ToList();
            return model;
        }

        // Creating Customer
        public object CreateCustomer(CustomerVM vmodel)
        {
            var customerCount = _db.Customers.Count();
            var customrUniqueNumberPrefix = _db.ApplicationSettings.FirstOrDefault().CustomerNumberPrefix;
            if (!customrUniqueNumberPrefix.EndsWith("/"))
                customrUniqueNumberPrefix = customrUniqueNumberPrefix + "/";


            Customer model = new Customer()
            {
                Firstname = vmodel.Firstname,
                CustomerUniqueID = customrUniqueNumberPrefix + (customerCount + 1).ToString("D6"),
                Lastname = vmodel.Lastname,
                DOB = DateTime.Now,
                Email = vmodel.Email,
                PhoneNumber = vmodel.PhoneNumber,
                Address = vmodel.Address,
                Gender = vmodel.Gender,
                IsDeleted = false,
                DateCreated = DateTime.Now,
                CreatedByID = Global.AuthenticatedUserID,
            };
            _db.Customers.Add(model);
            _db.SaveChanges();

            return new { hasSaved = true, customerUniqueID = model.CustomerUniqueID };
        }

        // Getting Customer
        public CustomerVM GetCustomer(int ID)
        {
            var model = _db.Customers.Where(x => x.Id == ID).Select(b => new CustomerVM()
            {
                Id = b.Id,
                CustomerUniqueID = b.CustomerUniqueID,
                Firstname = b.Firstname,
                Lastname = b.Lastname,
                Gender = b.Gender,
                DOB = DateTime.Now,
                Religion = b.Religion,
                Address = b.Address,
                PhoneNumber = b.PhoneNumber,
                Email = b.Email
            }).FirstOrDefault();
            return model;
        }
        public CustomerReportVM CustomerReport(CustomerReportVM vmodel)
        {

            var model = new CustomerReportVM();
            List<CustomerReportVM> records = new List<CustomerReportVM>();
            List<Customer> lists = new List<Customer>();

            if (vmodel.StartDate != null && vmodel.EndDate != null)
            {
                var startDate = new DateTime(vmodel.StartDate.Value.Year, vmodel.StartDate.Value.Month, vmodel.StartDate.Value.Day, DateTime.MinValue.Hour, DateTime.MinValue.Minute, DateTime.MinValue.Second);
                var endDate = new DateTime(vmodel.EndDate.Value.Year, vmodel.EndDate.Value.Month, vmodel.EndDate.Value.Day, DateTime.MaxValue.Hour, DateTime.MaxValue.Minute, DateTime.MaxValue.Second);

                lists = _db.Customers.Where(x => x.DateCreated >= startDate && x.DateCreated <= endDate && !x.IsDeleted).ToList();
            }
            int count = 0;
            foreach (var b in lists)
            {
                count = count + 1;
                var record = new CustomerReportVM()
                {
                    CustomerUniqueID = b.CustomerUniqueID,
                    Count = count,
                    Firstname = b.Firstname,
                    Lastname = b.Lastname,
                    Gender = b.Gender,
                    PhoneNumber = b.PhoneNumber,
                    DateCreated = b.DateCreated,
                    RegisteredBy = b.CreatedBy?.Username,
                };
                records.Add(record);
                model.TableData = records;
               
            }
            return model;
        }
        // Editting and updating Customer
        public bool EditCustomer(CustomerVM vmodel)
        {
            bool HasSaved = false;
            var model = _db.Customers.Where(x => x.Id == vmodel.Id).FirstOrDefault();
            model.Firstname = vmodel.Firstname;
            model.Lastname = vmodel.Lastname;
            model.Email = vmodel.Email;
            model.PhoneNumber = vmodel.PhoneNumber;
            model.Gender = vmodel.Gender;
            model.Address = vmodel.Address;
            model.DOB = DateTime.Now;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Deleting Customer
        public bool DeleteCustomer(int ID)
        {
            bool HasDeleted = false;
            var model = _db.Customers.Where(x => x.Id == ID).FirstOrDefault();
            model.IsDeleted = true;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasDeleted = true;
            return HasDeleted;
        }

        // Get Customer by UniqueID
        public CustomerVM GetCustomer(string unqiueID)
        {
            var model = _db.Customers.Where(x => x.CustomerUniqueID == unqiueID || x.PhoneNumber == unqiueID).Select(b => new CustomerVM()
            {
                Id = b.Id,
                CustomerUniqueID = b.CustomerUniqueID,
                Firstname = b.Firstname,
                Lastname = b.Lastname,
                Gender = b.Gender,
                DOB = DateTime.Now,
                Religion = b.Religion,
                Address = b.Address,
                PhoneNumber = b.PhoneNumber,
                Email = b.Email
            }).FirstOrDefault();
            return model;
        }
        public CustomerVM SearchCustomerWithIDorPhoneNumber(string value)
        {
            var model = _db.Customers.Where(x => (x.CustomerUniqueID == value || x.PhoneNumber == value) && x.IsDeleted == false).Select(b => new CustomerVM()
            {
                Id = b.Id,
                CustomerUniqueID = b.CustomerUniqueID,
                Firstname = b.Firstname,
                Lastname = b.Lastname,
                Gender = b.Gender,
                DOB = DateTime.Now,
                Religion = b.Religion,
                Address = b.Address,
                PhoneNumber = b.PhoneNumber,
                Email = b.Email
            }).FirstOrDefault();
            return model;
        }
        public List<string> GetCustomerOrPhoneAutoComplete(string term)
        {
            List<string> list;
            list = _db.Customers.Where(x => !x.IsDeleted && (x.CustomerUniqueID.StartsWith(term) || x.CustomerUniqueID.EndsWith(term) || x.PhoneNumber.StartsWith(term) || x.PhoneNumber.EndsWith(term))).Select(b => b.CustomerUniqueID).ToList();
            return list;
        }

    }
}