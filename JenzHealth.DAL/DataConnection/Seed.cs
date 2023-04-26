using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.DAL.DataConnection
{
    public class Seed
    {
        public static void DatabaseSeed(DatabaseEntities context)
        {
            context.ApplicationSettings.AddOrUpdate(x => x.Id,
          new Entity.ApplicationSettingsRecord()
          {
              Id = 1,
              AppName = "TAILOR APP",
              AllowSetupRouteAccess = true,
              CodeGenSeed = 7,
              SessionTimeOut = 15,
              CustomerNumberPrefix = "TMS/CUST/",
              EnableSpecimentCollectedBy = true,
              EnablePartPayment = true
          });
            context.Roles.AddOrUpdate(x => x.Id,
                new Entity.Role()
                {
                    Id = 1,
                    Description = "Super Administrator",
                    DateCreated = DateTime.Now,
                    IsDeleted = false
                }
                ,
                 new Entity.Role()
                 {
                     Id = 2,
                     Description = "Administrator",
                     DateCreated = DateTime.Now,
                     IsDeleted = false
                 },
                  new Entity.Role()
                  {
                      Id = 3,
                      Description = "Tailor",
                      DateCreated = DateTime.Now,
                      IsDeleted = false
                  }
                );

            context.Users.AddOrUpdate(x => x.Id,
                new Entity.User()
                {
                    Id = 1,
                    Firstname = "Joe",
                    Lastname = "Parker",
                    Username = "Admin",
                    Email = "admin@WebApp.com",
                    IsActive = true,
                    RoleID = 1,
                    Password = WebApp.DAL.Helpers.CustomEnrypt.Encrypt("Legendary"),
                    DateCreated = DateTime.Now
                }
                );
      
            context.SaveChanges();
        }
    }
}
