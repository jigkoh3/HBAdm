using DAL.Entity.Models;
using DAL.Repository.Providers.EntityFramework;
using System.Data.Entity;

namespace DAL.Data
{
    public partial class DataContext : DbContextBase
    {
        public DataContext() :
            base("DefaultConnection")
        {
            //Database.SetInitializer<DataContext>(null);
            //Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Configuration>());
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DataContext>());
        }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RequestMember>().ToTable("RequestMember", "dbo");
            modelBuilder.Entity<Hospital>().ToTable("Hospital", "dbo");
        }
    }
}
