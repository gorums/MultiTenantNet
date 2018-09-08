
namespace MultiTenantNet.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Obtain the async list of Entities
        /// </summary>
        /// <returns>Async list of Entities</returns>
        Task<List<TEntity>> GetAllAsync();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetAllQueryable();

        /// <summary>
        /// Obtain the async list of Entities
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> GetAllByCriteriaAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetAllQueryableByCriteria(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Obtain the async list of Entities by criteria
        /// </summary>
        /// <param name="predicate">A criteria</param>
        /// <returns>Async list of Entities by criteria</returns>
        Task<TEntity> GetByCriteriaAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Obtain the entity by Id
        /// </summary>
        /// <typeparam name="TEntity">The generic Entity</typeparam>
        /// <returns>An Entity</returns>
        Task<TEntity> FindAsync(Guid id);

        /// <summary>
        /// Obtain the entity by Id
        /// </summary>
        /// <typeparam name="TEntity">The generic Entity</typeparam>
        /// <returns>An Entity</returns>
        Task<TEntity> FindByCriteriaAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Add a new Entity into the context
        /// </summary>
        /// <param name="entity">>The new Entity</param>
        void Add(TEntity entity);

        /// <summary>
        ///  Add a list of Entities into the context
        /// </summary>
        /// <param name="entities"></param>
        void AddRange(List<TEntity> entities);

        /// <summary>
        /// Update an Entity
        /// </summary>
        /// <param name="entity">The Entity</param>
        void Update(TEntity entity);

        /// <summary>
        /// Update a List of Entities
        /// </summary>
        /// <param name="entities"></param>
        void UpdateRange(List<TEntity> entities);

        /// <summary>
        /// Delete an Entity
        /// </summary>
        /// <param name="entity">The Entity</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Delete a list of Entities
        /// </summary>
        /// <param name="entity"></param>
        void DeleteRange(List<TEntity> entities);
    }
}
