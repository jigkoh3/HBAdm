using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.Repository.Providers.EntityFramework
{
    public interface IDbContext : IDisposable
    {
        Guid InstanceId { get; }
        DbSet<T> Set<T>() where T : class;
        int SaveChanges();
        string connectionstring{get;}
        #region "Use with EF6"
        //Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        //Task<int> SaveChangesAsync();
        #endregion
        void SyncObjectState(object entity);
    }
}
