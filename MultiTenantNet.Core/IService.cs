
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
    public interface IService<TEntity>
    {
        /// <summary>
        /// Obtain the Entity using cache
        /// </summary>
        /// <returns>Entity by id</returns>
        Task<TEntity> FindAsync(Guid id);

        /// <summary>
        /// Obtain the Entity using cache
        /// </summary>
        /// <returns>Entity by id</returns>
        Task<TEntity> FindByCriteriaAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Obtain the async list of generic Entities by a criteria
        /// </summary>
        /// <param name="predicate">The criteria</param>
        /// <returns>The async list of generic Entities</returns>
        Task<TEntity> GetByCriteriaAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Obtain the async list of generic Entities
        /// </summary>
        /// <returns>The async list of generic Entities</returns>
        Task<List<TEntity>> GetAllAsync();

        /// <summary>
        /// Obtain the async queryable of generic Entities
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetAllQueryable();

        /// <summary>
        ///  Obtain the async list of generic Entities by criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetAllByCriteriaAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///  Obtain the async queryable of generic Entities by criteria
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetAllQueryableByCriteria(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Add a new Entity into the repository and do a commit
        /// </summary>
        /// <param name="entity">The generic Entity</param>
        /// <returns>The new Entity</returns>
        TEntity Add(TEntity entity);

        /// <summary>
        /// Add a new list of Entities into the repository and do a commit
        /// </summary>
        /// <param name="entities">The list of Entities</param>
        /// <returns>The list of Entities</returns>
        List<TEntity> AddRange(List<TEntity> entities);

        /// <summary>
        /// Add a new Entity async into the repository and do a commit
        /// </summary>
        /// <param name="entity">The generic Entity</param>
        /// <returns>The new Entity</returns>
        Task<TEntity> AddAsync(TEntity entity);

        /// <summary>
        /// Add a new list of Entities async into the repository and do a commit
        /// </summary>
        /// <param name="entity">The list of Entities</param>
        /// <returns>The list of Entities</returns>
        Task<List<TEntity>> AddRangeAsync(List<TEntity> entities);

        /// <summary>
        /// Update an Entity into the repository and do a commit
        /// </summary>
        /// <param name="entity">The generic Entity</param>
        /// <returns>The updated Entity</returns>
        TEntity Update(TEntity entity);

        /// <summary>
        /// Update a list of Entities into the repository and do a commit
        /// </summary>
        /// <param name="entity">The list of Entities</param>
        /// <returns>The list of Entities</returns>
        List<TEntity> UpdateRange(List<TEntity> entities);

        /// <summary>
        /// Update an Entity async into the repository and do a commit
        /// </summary>
        /// <param name="entity">The generic Entity</param>
        /// <returns>The updated Entity</returns>
        Task<TEntity> UpdateAsync(TEntity entity);

        /// <summary>
        /// Update a list of Entities async into the repository and do a commit
        /// </summary>
        /// <param name="entity">The list of Entities</param>
        /// <returns>The list of Entities</returns>
        Task<List<TEntity>> UpdateRangeAsync(List<TEntity> entities);

        /// <summary>
        /// Delete an Entity into the repository and do a commit
        /// </summary>
        /// <param name="entity">The generic Entity</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Delete a list of Entitiesinto the repository and do a commit
        /// </summary>
        /// <param name="entities">>The list of Entities</param>
        void DeleteRange(List<TEntity> entities);

        /// <summary>
        /// Delete an Entity async into the repository and do a commit
        /// </summary>
        /// <param name="entity">The generic Entity</param>
        /// <returns>The deleted Entity</returns>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Delete a list of Entities async into the repository and do a commit
        /// </summary>
        /// <param name="entities">>The list of Entities</param>
        /// <returns></returns>
        Task DeleteRangeAsync(List<TEntity> entities);
    }
}
