
namespace MultiTenantNet.Core
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    /// <summary>
    /// This interface define a Unit of Work patter
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Obtain a generic repository
        /// </summary>
        /// <typeparam name="TEntity">The Entity</typeparam>
        /// <typeparam name="TKey">The type of the key</typeparam>
        /// <returns>A generic repository</returns>
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;

        /// <summary>
        /// Begin a context transaction
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Do the context commit
        /// </summary>
        /// <returns>An identifier value</returns>
        int Commit();

        /// <summary>
        /// Do the context commit async
        /// </summary>
        /// <returns>An identifier value</returns>
        Task<int> CommitAsync();

        /// <summary>
        /// Rollback the context transaction
        /// </summary>
        void Rollback();
    }
}
