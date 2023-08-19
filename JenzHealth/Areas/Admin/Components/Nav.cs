using WebApp.Areas.Admin.Services;
using WebApp.DAL.DataConnection;
using WebApp.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.Components
{
    public static class Nav
    {
        private static DatabaseEntities db = new DatabaseEntities();
        private static UserService _userService = new UserService();
        private static string defaultIcon = "";
        public static List<Menu> AllMenus = new List<Menu>();
        public static List<Menu> ApplicationMenu = new List<Menu>()
        {
              // Home
            new Menu(url: "/Admin/Home/Home",stringText:"Home", icon: "home", isMenu: false, claim: "home", childMenus: new List<Menu>(){ }),

           
           

                 // Customer
            new Menu(url: "#",stringText:"Customer Management", icon: "accessible", isMenu: true,claim: "customer", childMenus: new List<Menu>(){
                new Menu(url: "/Admin/Customer/ManageCustomers",stringText:"Customers", icon: null ?? defaultIcon, isMenu: true,claim: "customer.managecustomers", childMenus: null),
            }),

              // Account
                new Menu(url: "#",stringText:"Account Management", icon: "money", isMenu: true,claim: "account", childMenus: new List<Menu>(){
                new Menu(url: "/Admin/Payment/Billings",stringText:"Billings", icon: null ?? defaultIcon, isMenu: true,claim: "account.billing", childMenus: null),
                new Menu(url: "/Admin/Payment/CashCollections",stringText:"Cash Collections", icon: null ?? defaultIcon, isMenu: true,claim: "account.cashcollection", childMenus: null),
            //  new Menu(url: "/Admin/Payment/DepositeCollections",stringText:"Deposite Collections", icon: null ?? defaultIcon, isMenu: true,claim: "account.depositecollection", childMenus: null),
                new Menu(url: "/Admin/Payment/Transactions",stringText:"Transactions", icon: null ?? defaultIcon, isMenu: true,claim: "account.transactions", childMenus: null),
                new Menu(url: "/Admin/Payment/PartPayment",stringText:"Part Payment", icon: null ?? defaultIcon, isMenu: true,claim: "account.partpayment", childMenus: null),
                new Menu(url: "/Admin/Payment/Waivers",stringText:"Waivers", icon: null ?? defaultIcon, isMenu: true,claim: "account.waivers", childMenus: null),
                new Menu(url: "/Admin/Payment/Refunds",stringText:"Refunds", icon: null ?? defaultIcon, isMenu: true,claim: "account.refunds", childMenus: null),
                new Menu(url: "/Admin/Payment/RecieptCancellations",stringText:"Reciept Cancellation", icon: null ?? defaultIcon, isMenu: true,claim: "account.recieptcancellation", childMenus: null),
               }),

             //Activity
              new Menu(url: "#",stringText:"Activity Management", icon: "accessible", isMenu: true,claim: "activity", childMenus: new List<Menu>(){
                new Menu(url: "/Admin/Activity/TakeMeasurement",stringText:"Take Measurement", icon: null ?? defaultIcon, isMenu: true,claim: "activity.takemeasurement", childMenus: null),
                new Menu(url: "/Admin/Activity/ClothStatus",stringText:"Cloth Status", icon: null ?? defaultIcon, isMenu: true,claim: "activity.clothstatus", childMenus: null),
                new Menu(url: "/Admin/Activity/RequestTracker",stringText:"Cloth Tracking/Collection", icon: null ?? defaultIcon, isMenu: true,claim: "report.managerequesttracker", childMenus: null),
            }),

           

                // Laboratory
            //new Menu(url: "#",stringText:"Laboratory Management", icon: "apartment", isMenu: true,claim: "laboratory", childMenus: new List<Menu>(){
            //    new Menu(url: "/Admin/Laboratory/ParameterSetups",stringText:"Parameter Setups", icon: null ?? defaultIcon, isMenu: true,claim: "laboratory.parametersetups", childMenus: null),
            //    new Menu(url: "/Admin/Laboratory/SpecimenCollections",stringText:"Specimen Collections", icon: null ?? defaultIcon, isMenu: true,claim: "laboratory.specimencollection", childMenus: null),
            //    new Menu(url: "/Admin/Laboratory/Preparations",stringText:"Preparations", icon: null ?? defaultIcon, isMenu: true,claim: "laboratory.Preparations", childMenus: null),
            //    new Menu(url: "/Admin/Laboratory/ResultApproval",stringText:"Result Approval", icon: null ?? defaultIcon, isMenu: true,claim: "laboratory.ResultApproval", childMenus: null),
            //}),

             // Seed
            //  new Menu(url: "#",stringText:"Seed Management", icon: "settings", isMenu: true, claim: "user", childMenus: new List<Menu>(){
            //    new Menu(url: "/Admin/Seed/ManageRevenueDepartments",stringText:"Revenue Departments", icon: null ?? defaultIcon, isMenu: true,claim: "seed.revenuedepartment", childMenus: null),
            //    new Menu(url: "/Admin/Seed/ManageServiceDepartments",stringText:"Service Departments", icon: null ?? defaultIcon, isMenu: true, claim: "seed.servicedepartment",childMenus: null),
            //    new Menu(url: "/Admin/Seed/ManageServices",stringText:"Services", icon: null ?? defaultIcon, isMenu: true, claim: "seed.Services",childMenus: null),
            //    new Menu(url: "/Admin/Seed/ManageSpecimens",stringText:"Specimens", icon: null ?? defaultIcon, isMenu: true, claim: "seed.specimens",childMenus: null),
            //    new Menu(url: "/Admin/Seed/ManageOrganisms",stringText:"Organism", icon: null ?? defaultIcon, isMenu: true, claim: "seed.organisms",childMenus: null),
            //    new Menu(url: "/Admin/Seed/ManageAntiBiotics",stringText:"AntiBiotics", icon: null ?? defaultIcon, isMenu: true, claim: "seed.antibiotics",childMenus: null),
            //    new Menu(url: "/Admin/Seed/ManageReferrers",stringText:"Referrers", icon: null ?? defaultIcon, isMenu: true, claim: "seed.referrers",childMenus: null),
            //    new Menu(url: "/Admin/Seed/ManagePriviledges",stringText:"Priviledges", icon: null ?? defaultIcon, isMenu: true, claim: "seed.priviledges",childMenus: null),
            //    new Menu(url: "/Admin/Seed/ManageTemplates",stringText:"Templates", icon: null ?? defaultIcon, isMenu: true, claim: "seed.templates",childMenus: null),
            //    new Menu(url: "/Admin/Seed/ManageVendors",stringText:"Vendors", icon: null ?? defaultIcon, isMenu: true, claim: "seed.vendors",childMenus: null),
            //}),

              
            // Report
            new Menu(url: "#",stringText:"Report Management", icon: "money", isMenu: true,claim: "Report", childMenus: new List<Menu>(){
               // new Menu(url: "/Admin/Report/ClothCollectors",stringText:"Cloth Collectors", icon: null ?? defaultIcon, isMenu: true,claim: "report.labresultcollectors", childMenus: null),
                new Menu(url: "/Admin/Report/CustomerReport",stringText:"Customer Report", icon: null ?? defaultIcon, isMenu: true,claim: "report.customerreport", childMenus: null),
                new Menu(url: "/Admin/Report/PaymentReport",stringText:"Payment Report", icon: null ?? defaultIcon, isMenu: true,claim: "report.paymentreport", childMenus: null),
                new Menu(url: "/Admin/Report/EarnedRevenueReport",stringText:"Earned Revenue Report", icon: null ?? defaultIcon, isMenu: true,claim: "report.earnedrevenuereport", childMenus: null),
                new Menu(url: "/Admin/Report/SharedRevenueReport",stringText:"Shared Revenue Report", icon: null ?? defaultIcon, isMenu: true,claim: "report.sharedrevenuereport", childMenus: null),
            }),

              // User
            new Menu(url: "#",stringText:"User Management", icon: "&#xE7FD;", isMenu: true, claim: "user", childMenus: new List<Menu>(){
                new Menu(url: "/Admin/User/Manage",stringText:"Manage Users", icon: null ?? defaultIcon, isMenu: true,claim: "user.manageusers", childMenus: null),
                new Menu(url: "/Admin/User/ManageRoles",stringText:"Manage Roles", icon: null ?? defaultIcon, isMenu: true, claim: "user.manageroles",childMenus: null),
            }),

            // Settings
            new Menu(url: "#",stringText:"Settings Management", icon: "settings", isMenu: true,claim: "settings", childMenus: new List<Menu>(){
                new Menu(url: "/Admin/ApplicationSettings/Manage",stringText:"Basic", icon: null ?? defaultIcon, isMenu: true,claim: "settings.manageapplicationsettings", childMenus: null),
                new Menu(url: "/Admin/ApplicationSettings/ManageClothTypes",stringText:"Cloth Type Setup", icon: null ?? defaultIcon, isMenu: true,claim: "settings.clothtype", childMenus: null),
                new Menu(url: "/Admin/ApplicationSettings/ManageMeasurements",stringText:"Measurement Setup ", icon: null ?? defaultIcon, isMenu: true,claim: "settings.measurement", childMenus: null),
                new Menu(url: "/Admin/ApplicationSettings/ManageClothTypeMeasurements",stringText:"Cloth Type Measurement Setup", icon: null ?? defaultIcon, isMenu: true,claim: "settings.clothtypemeasurement", childMenus: null),
                new Menu(url: "/Admin/ApplicationSettings/ManageSettlementSetup",stringText:"Settlement Setup", icon: null ?? defaultIcon, isMenu: true,claim: "settings.settlementsetup", childMenus: null),

            }),
        };
        public static List<Menu> AppMenu = new List<Menu>()
          {
              // Home
            new Menu(url: "/Admin/Home/Home",stringText:"Home", icon: "home", isMenu: false, claim: "home", childMenus: new List<Menu>(){ }),

          
             // Customer
            new Menu(url: "#",stringText:"Customer Management", icon: "accessible", isMenu: true,claim: "customer", childMenus: new List<Menu>(){
                new Menu(url: "/Admin/Customer/ManageCustomers",stringText:"Customers", icon: null ?? defaultIcon, isMenu: true,claim: "customer.managecustomers", childMenus: null),
            }),

               // Account
                new Menu(url: "#",stringText:"Account Management", icon: "money", isMenu: true,claim: "account", childMenus: new List<Menu>(){
                new Menu(url: "/Admin/Payment/Billings",stringText:"Billings", icon: null ?? defaultIcon, isMenu: true,claim: "account.billing", childMenus: null),
                new Menu(url: "/Admin/Payment/CashCollections",stringText:"Cash Collections", icon: null ?? defaultIcon, isMenu: true,claim: "account.cashcollection", childMenus: null),
            //  new Menu(url: "/Admin/Payment/DepositeCollections",stringText:"Deposite Collections", icon: null ?? defaultIcon, isMenu: true,claim: "account.depositecollection", childMenus: null),
                new Menu(url: "/Admin/Payment/Transactions",stringText:"Transactions", icon: null ?? defaultIcon, isMenu: true,claim: "account.transactions", childMenus: null),
                new Menu(url: "/Admin/Payment/PartPayment",stringText:"Part Payment", icon: null ?? defaultIcon, isMenu: true,claim: "account.partpayment", childMenus: null),
                new Menu(url: "/Admin/Payment/Waivers",stringText:"Waivers", icon: null ?? defaultIcon, isMenu: true,claim: "account.waivers", childMenus: null),
                new Menu(url: "/Admin/Payment/Refunds",stringText:"Refunds", icon: null ?? defaultIcon, isMenu: true,claim: "account.refunds", childMenus: null),
                new Menu(url: "/Admin/Payment/RecieptCancellations",stringText:"Reciept Cancellation", icon: null ?? defaultIcon, isMenu: true,claim: "account.recieptcancellation", childMenus: null),
               }),

             //Activity
              new Menu(url: "#",stringText:"Activity Management", icon: "accessible", isMenu: true,claim: "activity", childMenus: new List<Menu>(){
                new Menu(url: "/Admin/Activity/TakeMeasurement",stringText:"Take Measurement", icon: null ?? defaultIcon, isMenu: true,claim: "activity.takemeasurement", childMenus: null),
                new Menu(url: "/Admin/Activity/ClothStatus",stringText:"Cloth Status", icon: null ?? defaultIcon, isMenu: true,claim: "activity.clothstatus", childMenus: null),
                new Menu(url: "/Admin/Activity/RequestTracker",stringText:"Cloth Tracking/Collection", icon: null ?? defaultIcon, isMenu: true,claim: "report.managerequesttracker", childMenus: null),
            }),

              
           

              // Laboratory
            //new Menu(url: "#",stringText:"Laboratory Management", icon: "apartment", isMenu: true,claim: "laboratory", childMenus: new List<Menu>(){
            //    new Menu(url: "/Admin/Laboratory/ParameterSetups",stringText:"Parameter Setups", icon: null ?? defaultIcon, isMenu: true,claim: "laboratory.parametersetups", childMenus: null),
            //    new Menu(url: "/Admin/Laboratory/SpecimenCollections",stringText:"Specimen Collections", icon: null ?? defaultIcon, isMenu: true,claim: "laboratory.specimencollection", childMenus: null),
            //    new Menu(url: "/Admin/Laboratory/Preparations",stringText:"Preparations", icon: null ?? defaultIcon, isMenu: true,claim: "laboratory.Preparations", childMenus: null),
            //    new Menu(url: "/Admin/Laboratory/ResultApproval",stringText:"Result Approval", icon: null ?? defaultIcon, isMenu: true,claim: "laboratory.ResultApproval", childMenus: null),
            //}),

             // Seed
            //new Menu(url: "#",stringText:"Seed Management", icon: "settings", isMenu: true, claim: "user", childMenus: new List<Menu>(){
            //    new Menu(url: "/Admin/Seed/ManageRevenueDepartments",stringText:"Revenue Departments", icon: null ?? defaultIcon, isMenu: true,claim: "seed.revenuedepartment", childMenus: null),
            //    new Menu(url: "/Admin/Seed/ManageServiceDepartments",stringText:"Service Departments", icon: null ?? defaultIcon, isMenu: true, claim: "seed.servicedepartment",childMenus: null),
            //    new Menu(url: "/Admin/Seed/ManageServices",stringText:"Services", icon: null ?? defaultIcon, isMenu: true, claim: "seed.Services",childMenus: null),
            //    new Menu(url: "/Admin/Seed/ManageSpecimens",stringText:"Specimens", icon: null ?? defaultIcon, isMenu: true, claim: "seed.specimens",childMenus: null),
            //    new Menu(url: "/Admin/Seed/ManageOrganisms",stringText:"Organism", icon: null ?? defaultIcon, isMenu: true, claim: "seed.organisms",childMenus: null),
            //    new Menu(url: "/Admin/Seed/ManageAntiBiotics",stringText:"AntiBiotics", icon: null ?? defaultIcon, isMenu: true, claim: "seed.antibiotics",childMenus: null),
            //    new Menu(url: "/Admin/Seed/ManageReferrers",stringText:"Referrers", icon: null ?? defaultIcon, isMenu: true, claim: "seed.referrers",childMenus: null),
            //    new Menu(url: "/Admin/Seed/ManagePriviledges",stringText:"Priviledges", icon: null ?? defaultIcon, isMenu: true, claim: "seed.priviledges",childMenus: null),
            //    new Menu(url: "/Admin/Seed/ManageTemplates",stringText:"Templates", icon: null ?? defaultIcon, isMenu: true, claim: "seed.templates",childMenus: null),
            //    new Menu(url: "/Admin/Seed/ManageVendors",strinnhgText:"Vendors", icon: null ?? defaultIcon, isMenu: true, claim: "seed.vendors",childMenus: null),
            //}),

             // Report
            new Menu(url: "#",stringText:"Report Management", icon: "money", isMenu: true,claim: "Report", childMenus: new List<Menu>(){
                //new Menu(url: "/Admin/Report/ClothCollectors",stringText:"Cloth Collectors", icon: null ?? defaultIcon, isMenu: true,claim: "report.labresultcollectors", childMenus: null),
                new Menu(url: "/Admin/Report/CustomerReport",stringText:"Customer Report", icon: null ?? defaultIcon, isMenu: true,claim: "report.customerreport", childMenus: null),
                new Menu(url: "/Admin/Report/PaymentReport",stringText:"Payment Report", icon: null ?? defaultIcon, isMenu: true,claim: "report.paymentrevenuereport", childMenus: null),
                new Menu(url: "/Admin/Report/EarnedRevenueReport",stringText:"Earned Revenue Report", icon: null ?? defaultIcon, isMenu: true,claim: "report.earnedrevenuereport", childMenus: null),
                new Menu(url: "/Admin/Report/SharedRevenueReport",stringText:"Shared Revenue Report", icon: null ?? defaultIcon, isMenu: true,claim: "report.sharedrevenuereport", childMenus: null),
            }),

              // User
            new Menu(url: "#",stringText:"User Management", icon: "&#xE7FD;", isMenu: true, claim: "user", childMenus: new List<Menu>(){
             new Menu(url: "/Admin/User/Manage",stringText:"Manage Users", icon: null ?? defaultIcon, isMenu: true,claim: "user.manageusers", childMenus: null),
             new Menu(url: "/Admin/User/ManageRoles",stringText:"Manage Roles", icon: null ?? defaultIcon, isMenu: true, claim: "user.manageroles",childMenus: null),
            }),

            // Settings
            new Menu(url: "#",stringText:"Settings Management", icon: "settings", isMenu: true,claim: "settings", childMenus: new List<Menu>(){
                new Menu(url: "/Admin/ApplicationSettings/Manage",stringText:"Basic", icon: null ?? defaultIcon, isMenu: true,claim: "settings.manageapplicationsettings", childMenus: null),
                new Menu(url: "/Admin/ApplicationSettings/ManageClothTypes",stringText:"Cloth Type Setup", icon: null ?? defaultIcon, isMenu: true,claim: "settings.clothtype", childMenus: null),
                new Menu(url: "/Admin/ApplicationSettings/ManageMeasurements",stringText:"Measurement Setup ", icon: null ?? defaultIcon, isMenu: true,claim: "settings.measurement", childMenus: null),
                new Menu(url: "/Admin/ApplicationSettings/ManageClothTypeMeasurements",stringText:"Cloth Type Measurement Setup", icon: null ?? defaultIcon, isMenu: true,claim: "settings.clothtypemeasurement", childMenus: null),
                new Menu(url: "/Admin/ApplicationSettings/ManageSettlementSetup",stringText:"Settlement Setup", icon: null ?? defaultIcon, isMenu: true,claim: "settings.settlementsetup", childMenus: null),

            }),
        };

        private static void GetAllMenu(List<Menu> menus, List<Menu> applicationMenus)
        {
            foreach (var menu in applicationMenus)
            {
                menus.Add(menu);
                if (menu._childMenus != null)
                {
                    GetAllMenu(AllMenus, menu._childMenus);
                }
            }
        }
        public static void UpdateAllMenus()
        {
            GetAllMenu(AllMenus, AppMenu);
        }

        public static bool CheckPermissionExist(string _stringText)
        {
            var exist = db.Permissions.Where(x => x.Description == _stringText).FirstOrDefault();
            if (exist != null)
                return true;
            else
                return false;
        }
        public static void StorePermissions(List<Menu> menuViewModels)
        {
            List<Permission> model = new List<Permission>();

            foreach (var menu in menuViewModels)
            {
                if (!CheckPermissionExist(menu._stringText))
                {
                    var permission = new Permission()
                    {
                        Description = menu._stringText,
                        Claim = menu._claim,
                        Url = menu._url
                    };
                    model.Add(permission);
                    if (menu._childMenus.Count > 0)
                    {
                        foreach (var submenu in menu._childMenus)
                        {
                            if (!CheckPermissionExist(submenu._stringText))
                            {
                                var childpermission = new Permission()
                                {
                                    Description = submenu._stringText,
                                    Claim = submenu._claim,
                                    Url = submenu._url
                                };
                                model.Add(childpermission);
                            }
                        }
                    }
                    db.Permissions.AddRange(model);
                }
                else
                {
                    var permission = db.Permissions.Where(x => x.Description == menu._stringText).FirstOrDefault();
                    permission.Description = menu._stringText;
                    permission.Claim = menu._claim;
                    permission.Url = menu._url;
                    db.Entry(permission).State = System.Data.Entity.EntityState.Modified;

                    if (menu._childMenus.Count > 0)
                    {
                        foreach (var submenu in menu._childMenus)
                        {
                            if (!CheckPermissionExist(submenu._stringText))
                            {
                                var childpermission = new Permission()
                                {
                                    Description = submenu._stringText,
                                    Claim = submenu._claim,
                                    Url = submenu._url
                                };
                                db.Permissions.Add(childpermission);
                            }
                            else
                            {
                                var childpermission = db.Permissions.Where(x => x.Description == submenu._stringText).FirstOrDefault();
                                childpermission.Description = submenu._stringText;
                                childpermission.Claim = submenu._claim;
                                childpermission.Url = submenu._url;
                                db.Entry(childpermission).State = System.Data.Entity.EntityState.Modified;
                            }
                        }
                    }
                }
                db.SaveChanges();
            }
        }

        public static void GetRolePermissionMenu(List<Menu> applicationMenus, List<RolePermission> permissionMenus)
        {
            foreach (var appmenu in applicationMenus)
            {
                var permissionRow = permissionMenus.FirstOrDefault(x => x.Permission == appmenu._stringText);

                appmenu.isAssigned = permissionRow == null ? false : permissionRow.IsAssigned;
                if (appmenu._childMenus.Count > 0)
                {
                    foreach (var subappmenu in appmenu._childMenus)
                    {
                        var permission = permissionMenus.FirstOrDefault(x => x.Permission == subappmenu._stringText);
                        if (permission != null)
                            subappmenu.isAssigned = permission.IsAssigned;
                    }
                    appmenu.isAssigned = appmenu._childMenus.Select(x => x.isAssigned).Contains(true);
                }
            }
        }
        public static List<Menu> GetAssignedPermission(int ID)
        {
            var CheckPermissionExists = db.RolePermissions.Where(x => x.RoleID == ID).Count();
            List<Menu> appmenus = new List<Menu>();
            appmenus = Nav.AppMenu;
            foreach (var menu in appmenus)
            {
                menu.isAssigned = db.RolePermissions.FirstOrDefault(x => x.Permission == menu._stringText && x.RoleID == ID).IsAssigned;
                if (menu._childMenus.Count > 0)
                {
                    foreach (var submenu in menu._childMenus)
                    {
                        submenu.isAssigned = db.RolePermissions.FirstOrDefault(x => x.Permission == submenu._stringText && x.RoleID == ID).IsAssigned;
                    }
                }
            }
            return appmenus;
        }

        public static void SavePermissionForRole(int roleID)
        {
            UpdateAllMenus();
            foreach (var menu in AllMenus)
            {
                if (CheckPermissionExist(roleID, menu._stringText) == 0)
                {
                    var rolePermission = new RolePermission();
                    rolePermission.PermissionID = db.Permissions.FirstOrDefault(x => x.Description == menu._stringText).Id;
                    rolePermission.Permission = menu._stringText;
                    rolePermission.RoleID = roleID;
                    rolePermission.IsAssigned = false;
                    rolePermission.IsDeleted = false;
                    db.RolePermissions.Add(rolePermission);
                }
                db.SaveChanges();
            }
        }
        public static int CheckPermissionExist(int RoleID, string Permission)
        {
            int result;
            result = db.RolePermissions.Where(x => x.RoleID == RoleID && x.Permission == Permission).Count();
            return result;
        }

        public static void AssignPermission(string permission, int roleID, bool isAssigned)
        {
            var RolePermission = db.RolePermissions.Where(p => p.RoleID == roleID && p.PermissionID == db.Permissions.FirstOrDefault(x => x.Description == permission).Id).FirstOrDefault();
            if (RolePermission != null)
            {
                RolePermission.IsAssigned = isAssigned;
                db.Entry(RolePermission).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public static bool CheckAuthorization(string resourceUrl)
        {
            var currentUser = _userService.GetCurrentUser();
            var permissionResourceURLs = db.RolePermissions.Where(x => x.IsDeleted == false && x.RoleID == currentUser.RoleID && x.IsAssigned == true).Select(b => b.Permissions.Url).ToList();
            if (permissionResourceURLs.Contains(resourceUrl))
                return true;
            else
                return false;
        }
    }
}