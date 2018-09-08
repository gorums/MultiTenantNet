
namespace MultiTenantNet.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// This interface define an abstract DbContext
    /// </summary>
    public interface IDbContext
    {
        /// <summary>
        /// Add an Entity
        /// </summary>
        /// <typeparam name="TEntity">The generic Entity</typeparam>
        /// <param name="entity">The Entity</param>
        void SetAsAdded<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Add an List of Entities
        /// </summary>
        /// <typeparam name="TEntity">The generic Entity</typeparam>
        /// <param name="entities">The List of Entities</param>
        void SetAsAdded<TEntity>(List<TEntity> entities) where TEntity : class;

        /// <summary>
        /// Update an Entity
        /// </summary>
        /// <typeparam name="TEntity">The generic Entity</typeparam>
        /// <param name="entity">The Entity</param>
        void SetAsModified<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Update an List of Entities
        /// </summary>
        /// <typeparam name="TEntity">The generic Entity</typeparam>
        /// <param name="entities">The List of Entities</param>
        void SetAsModified<TEntity>(List<TEntity> entities) where TEntity : class;

        /// <summary>
        /// Delete an Entity
        /// </summary>
        /// <typeparam name="TEntity">The generic Entity</typeparam>
        /// <param name="entity">The Entity</param>
        void SetAsDeleted<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Delete an List of Entities
        /// </summary>
        /// <typeparam name="TEntity">The generic Entity</typeparam>
        /// <param name="entities">The List of Entities</param>
        void SetAsDeleted<TEntity>(List<TEntity> entities) where TEntity : class;

        /// <summary>
        /// Obtain a list of async generic Entities
        /// </summary>
        /// <typeparam name="TEntity">The generic Entity</typeparam>
        /// <returns>A list of async generic Entities</returns>
        Task<List<TEntity>> ToListAsync<TEntity>() where TEntity : class;

        /// <summary>
        /// Obtain a list of async generic Entities by criteria
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        Task<List<TEntity>> ToListByCriteriaAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        /// <summary>
        /// Obtain a list query of async generic Entities
        /// </summary>
        /// <typeparam name="TEntity">The generic Entity</typeparam>
        /// <returns>A list query of async generic Entities</returns>
        IQueryable<TEntity> ToQueryable<TEntity>() where TEntity : class;

        /// <summary>
        /// Obtain a list query of async generic Entities by criteria
        /// </summary>
        /// <typeparam name="TEntity">The generic Entity</typeparam>
        /// <returns>A list query of async generic Entities</returns>
        IQueryable<TEntity> ToQueryableByCriteria<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        /// <summary>
        /// Obtain first or something async generic Entity
        /// </summary>
        /// <typeparam name="TEntity">The generic Entity</typeparam>
        /// <param name="predicate">A criteria</param>
        /// <returns>An Entity or default</returns>
        Task<TEntity> FirstOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        /// <summary>
        /// Obtain the entity on memory by Id
        /// </summary>
        /// <typeparam name="TEntity">The generic Entity</typeparam>
        /// <returns>An Entity</returns>
        Task<TEntity> FindAsync<TEntity>(Guid id) where TEntity : class;

        /// <summary>
        /// Obtain the entity on memory by criteria
        /// </summary>
        /// <typeparam name="TEntity">The generic Entity</typeparam>
        /// <returns>An Entity</returns>
        Task<TEntity> FindByCriteriaAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        /// <summary>
        /// Begin an transaction
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Do a commit
        /// </summary>
        /// <returns>An identifier value</returns>
        int Commit();

        /// <summary>
        ///  Do an async commit
        /// </summary>
        /// <returns>An identifier value</returns>
        Task<int> CommitAsync();

        /// <summary>
        /// Do a rollback
        /// </summary>
        void Rollback();
    }
}
