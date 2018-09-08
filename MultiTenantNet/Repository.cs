
namespace MultiTenantNet
{
    using MultiTenantNet.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// The DataBase Context
        /// </summary>
        private readonly IDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity, TKey}" /> class
        /// </summary>
        /// <param name="context">The implementation of Database Context <see cref="IDbContext" /></param>
        public Repository(IDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// <see cref="IRepository{TEntity,TKey}.GetAllAsync"/>
        /// </summary>
        /// <param name="disabled">Parameter disabled <see cref="IService{TEntity, TKey}.GetAllAsync(bool?,bool?)"/></param>
        /// <param name="trash">Parameter trash <see cref="IService{TEntity, TKey}.GetAllAsync(bool?,bool?)"/></param>
        /// <returns>Return <see cref="IRepository{TEntity, TKey}.GetAllAsync"/></returns>
        public Task<List<TEntity>> GetAllAsync()
        {
            return this.context.ToListAsync<TEntity>();
        }

        /// <summary>
        /// <see cref="IRepository{TEntity,TKey}.GetAllQueryableAsync"/>
        /// </summary>
        /// <returns>Return <see cref="IRepository{TEntity, TKey}.GetAllQueryableAsync"/></returns>
        public IQueryable<TEntity> GetAllQueryable()
        {
            return this.context.ToQueryable<TEntity>();
        }

        /// <summary>
        /// <see cref="IRepository{TEntity,TKey}.GetAllByCriteriaAsync"/>
        /// </summary>
        /// <param name="predicate">A criteria</param>
        /// <returns>Return <see cref="IRepository{TEntity, TKey}.GetAllByCriteriaAsync"/></returns>
        public Task<List<TEntity>> GetAllByCriteriaAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return this.context.ToListByCriteriaAsync<TEntity>(predicate);
        }

        /// <summary>
        /// <see cref="IRepository{TEntity}.GetAllQueryableByCriteria"/>
        /// </summary>
        /// <param name="predicate">A criteria</param>
        /// <returns>Return <see cref="IRepository{TEntity}.GetAllQueryableByCriteria"/></returns>
        public IQueryable<TEntity> GetAllQueryableByCriteria(Expression<Func<TEntity, bool>> predicate)
        {
            return this.context.ToQueryableByCriteria<TEntity>(predicate);
        }

        /// <summary>
        /// <see cref="IRepository{TEntity}.GetByCriteriaAsync(Expression{Func{TEntity, bool}})"/>
        /// </summary>
        /// <param name="predicate">Parameter <see cref="IRepository{TEntity}.GetByCriteriaAsync(Expression{Func{TEntity, bool}})"/></param>
        /// <returns>Return <see cref="IRepository{TEntity, TKey}.GetByCriteriaAsync(Expression{Func{TEntity, bool}})"/></returns>
        public Task<TEntity> GetByCriteriaAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return this.context.FirstOrDefaultAsync<TEntity>(predicate);
        }

        /// <summary>
        /// <see cref="IRepository{TEntity}.FindByCriteria(predicate)"/>
        /// </summary>
        /// <param name="predicate">Parameter <see cref="IRepository{TEntity}.FindByCriteria(predicate)"/></param>
        /// <returns>Return <see cref="IRepository{TEntity}.FindByCriteria(predicate)"/></returns>
        public Task<TEntity> FindByCriteriaAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return this.context.FindByCriteriaAsync<TEntity>(predicate);
        }
        /// <summary>
        /// <see cref="IRepository{TEntity}.Find(id)"/>
        /// </summary>
        /// <param name="id">Parameter <see cref="IRepository{TEntity}.Find(id)"/></param>
        /// <returns>Return <see cref="IRepository{TEntity}.Find(id)"/></returns>

        public Task<TEntity> FindAsync(Guid id)
        {
            return this.context.FindAsync<TEntity>(id);
        }

        /// <summary>
        /// <see cref="IRepository{TEntity}.Add(TEntity)"/>
        /// </summary>
        /// <param name="entity">Parameter <see cref="IRepository{TEntity}.Add(TEntity)"/></param>
        public void Add(TEntity entity)
        {
            this.context.SetAsAdded<TEntity>(entity);
        }

        /// <summary>
        /// <see cref="IRepository{TEntity}.AddRange(TEntity)"/
        /// </summary>
        /// <param name="entities">Parameter <see cref="IRepository{TEntity}.AddRange(TEntity)"/></param>
        public void AddRange(List<TEntity> entities)
        {
            this.context.SetAsAdded<TEntity>(entities);
        }

        /// <summary>
        /// <see cref="IRepository{TEntity}.Update(TEntity)"/>
        /// </summary>
        /// <param name="entity">Parameter <see cref="IRepository{TEntity}.Update(TEntity)"/></param>
        public void Update(TEntity entity)
        {
            this.context.SetAsModified<TEntity>(entity);
        }

        /// <summary>
        /// <see cref="IRepository{TEntity}.UpdateRange(TEntity)"/>
        /// </summary>
        /// <param name="entities">Parameter <see cref="IRepository{TEntity}.UpdateRange(TEntity)"/></param>
        public void UpdateRange(List<TEntity> entities)
        {
            this.context.SetAsModified<TEntity>(entities);
        }

        /// <summary>
        /// <see cref="IRepository{TEntity}.Delete(TEntity)"/>
        /// </summary>
        /// <param name="entity">Parameter <see cref="IRepository{TEntity}.Delete(TEntity)"/></param>
        public void Delete(TEntity entity)
        {
            this.context.SetAsDeleted<TEntity>(entity);
        }

        /// <summary>
        ///  <see cref="IRepository{TEntity}.DeleteRange(TEntity)"/>
        /// </summary>
        /// <param name="entities">Parameter <see cref="IRepository{TEntity}.DeleteRange(TEntity)"/></param>
        public void DeleteRange(List<TEntity> entities)
        {
            this.context.SetAsDeleted<TEntity>(entities);
        }
    }
}
