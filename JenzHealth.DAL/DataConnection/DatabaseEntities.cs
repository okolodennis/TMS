using WebApp.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.DAL.DataConnection
{
    public class DatabaseEntities : DbContext
    {
        public DatabaseEntities() : base("name=DatabaseEntities")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseEntities, WebApp.DAL.Migrations.Configuration>());
            base.Configuration.ProxyCreationEnabled = true;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ApplicationSettingsRecord> ApplicationSettings { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Billing> Billings { get; set; }
        public virtual DbSet<Waiver> Waivers { get; set; }
        public virtual DbSet<PartPayment> PartPayments { get; set; }
        public virtual DbSet<DepositeCollection> DepositeCollections { get; set; }
        public virtual DbSet<CashCollection> CashCollections { get; set; }
        public virtual DbSet<Shift> Shifts { get; set; }
        public virtual DbSet<Refund> Refunds { get; set; }
        public virtual DbSet<ClothType> ClothTypes { get; set; }
        public virtual DbSet<Measurement> Measurements { get; set; }
        public virtual DbSet<ClothTypeMeasurement> ClothTypeMeasurements { get; set; }
        public virtual DbSet<MeasurementCollection> MeasurementCollections { get; set; }
        public virtual DbSet<AssignedTailorToBilledCloth> AssignedTailorToBilledClothes { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<ResultApproval> ResultApprovals { get; set; }
        public virtual DbSet<SettlementSetup> SettlementSetups { get; set; }
 
    }
}
