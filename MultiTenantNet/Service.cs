
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
    public class Service<TEntity> : IService<TEntity> where TEntity : class
    {
        /// <summary>
        /// The generic repository <see cref="IRepository{TEntity, TKey}"/>
        /// </summary>
        private IRepository<TEntity> repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Service{TEntity, TKey}" /> class
        /// </summary>
        /// <param name="unitOfWork">The implementation of Unit Of Work pattern <see cref="IUnitOfWork" /></param>
        public Service(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;            
        }

        /// <summary>
        /// Gets or Sets the Unit Of Work pattern <see cref="IUnitOfWork" />
        /// </summary>
        public IUnitOfWork UnitOfWork { get; private set; }

        /// <summary>
        /// <see cref="IRepository{TEntity, TKey}.Find(TKey)"/>
        /// </summary>
        /// <param name="id">Parameter <see cref="IRepository{TEntity, TKey}.Find(TKey)"/></param>
        /// <returns>Return <see cref="IRepository{TEntity, TKey}.Find(TKey)"/></returns>
        public Task<TEntity> FindAsync(Guid id)
        {
            InitializeRepository();

            return this.repository.FindAsync(id);
        }

        /// <summary>
        /// <see cref="IRepository{TEntity, TKey}.Find(TKey)"/>
        /// </summary>
        /// <param name="id">Parameter <see cref="IRepository{TEntity, TKey}.Find(TKey)"/></param>
        /// <returns>Return <see cref="IRepository{TEntity, TKey}.Find(TKey)"/></returns>
        public Task<TEntity> FindByCriteriaAsync(Expression<Func<TEntity, bool>> predicate)
        {
            InitializeRepository();

            return this.repository.FindByCriteriaAsync(predicate);
        }

        /// <summary>
        /// <see cref="IService{TEntity}.GetAllAsync"/>
        /// </summary>
        /// <returns>Return <see cref="IService{TEntity}.GetAllAsync"/></returns>
        public Task<List<TEntity>> GetAllAsync()
        {
            InitializeRepository();

            return this.repository.GetAllAsync();
        }

        /// <summary>
        /// <see cref="IService{TEntity}.GetAllQueryableAsync"/>
        /// </summary>
        /// <returns>Return <see cref="IService{TEntity}.GetAllQueryableAsync"/></returns>
        public IQueryable<TEntity> GetAllQueryable()
        {
            InitializeRepository();

            return this.repository.GetAllQueryable();
        }

        /// <summary>
        /// <see cref="IService{TEntity}.GetAllByCriteriaAsync"/>
        /// </summary>
        /// <param name="predicate">Parameter <see cref="IService{TEntity}.GetAllByCriteriaAsync(Expression{Func{TEntity, bool}})"/></param>
        /// <returns>Return <see cref="IService{TEntity, TKey}.GetAllByCriteriaAsync"/></returns>
        public Task<List<TEntity>> GetAllByCriteriaAsync(Expression<Func<TEntity, bool>> predicate)
        {
            InitializeRepository();

            return this.repository.GetAllByCriteriaAsync(predicate);
        }

        /// <summary>
        /// <see cref="IService{TEntity}.GetAllQueryableByCriteriaAsync"/>
        /// </summary>
        /// <param name="predicate">Parameter <see cref="IService{TEntity}.GetAllQueryableByCriteriaAsync(Expression{Func{TEntity, bool}})"/></param>
        /// <returns>Return <see cref="IService{TEntity}.GetAllQueryableByCriteriaAsync"/></returns>
        public IQueryable<TEntity> GetAllQueryableByCriteria(Expression<Func<TEntity, bool>> predicate)
        {
            InitializeRepository();

            return this.repository.GetAllQueryableByCriteria(predicate);
        }

        /// <summary>
        /// <see cref="IService{TEntity}.GetByCriteriaAsync(Expression{Func{TEntity, bool}})"/>
        /// </summary>
        /// <param name="predicate">Parameter <see cref="IService{TEntity}.GetByCriteriaAsync(Expression{Func{TEntity, bool}})"/></param>
        /// <returns>Return <see cref="IService{TEntity}.GetByCriteriaAsync(Expression{Func{TEntity, bool}})"/></returns>
        public Task<TEntity> GetByCriteriaAsync(Expression<Func<TEntity, bool>> predicate)
        {
            InitializeRepository();

            return this.repository.GetByCriteriaAsync(predicate);
        }

        /// <summary>
        /// <see cref="IService{TEntity}.Add(TEntity)"/>
        /// </summary>
        /// <param name="entity">Parameter <see cref="IService{TEntity}.Add(TEntity)"/></param>
        /// <returns>Return <see cref="IService{TEntity}.Add(TEntity)"/></returns>
        public TEntity Add(TEntity entity)
        {
            InitializeRepository();

            this.repository.Add(entity);
            this.UnitOfWork.Commit();

            return entity;
        }

        /// <summary>
        /// <see cref="IService{TEntity}.AddRange(TEntity)"/>
        /// </summary>
        /// <param name="entities">Parameter <see cref="IService{TEntity}.AddRange(TEntity)"/></param>
        /// <returns>Return <see cref="IService{TEntity}.AddRange(TEntity)"/></returns>
        public List<TEntity> AddRange(List<TEntity> entities)
        {
            InitializeRepository();

            this.repository.AddRange(entities);
            this.UnitOfWork.Commit();

            return entities;
        }

        /// <summary>
        /// <see cref="IService{TEntity}.AddAsync(TEntity)"/>
        /// </summary>
        /// <param name="entity">Parameter <see cref="IService{TEntity}.AddAsync(TEntity)"/></param>
        /// <returns>Return <see cref="IService{TEntity}.AddAsync(TEntity)"/></returns>
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            InitializeRepository();

            this.repository.Add(entity);
            await this.UnitOfWork.CommitAsync();

            return entity;
        }

        /// <summary>
        /// <see cref="IService{TEntity}.AddRangeAsync(TEntity)"/>
        /// </summary>
        /// <param name="entities">>Parameter <see cref="IService{TEntity}.AddRangeAsync(TEntity)"/></param>
        /// <returns>Return <see cref="IService{TEntity}.AddRangeAsync(TEntity)"/></returns>
        public async Task<List<TEntity>> AddRangeAsync(List<TEntity> entities)
        {
            InitializeRepository();

            this.repository.AddRange(entities);
            await this.UnitOfWork.CommitAsync();

            return entities;
        }

        /// <summary>
        /// <see cref="IService{TEntity}.Update(TEntity)"/>
        /// </summary>
        /// <param name="entity">parameter <see cref="IService{TEntity}.Update(TEntity)"/></param>
        /// <returns>Return <see cref="IService{TEntity}.Update(TEntity)"/></returns>
        public TEntity Update(TEntity entity)
        {
            InitializeRepository();

            this.repository.Update(entity);
            this.UnitOfWork.Commit();

            return entity;
        }

        /// <summary>
        /// <see cref="IService{TEntity}.UpdateRange(TEntity)"/>
        /// </summary>
        /// <param name="entities">parameter <see cref="IService{TEntity}.UpdateRange(TEntity)"/></param>
        /// <returns>Return <see cref="IService{TEntity}.UpdateRange(TEntity)"/></returns>
        public List<TEntity> UpdateRange(List<TEntity> entities)
        {
            InitializeRepository();

            this.repository.UpdateRange(entities);
            this.UnitOfWork.Commit();

            return entities;
        }

        /// <summary>
        /// <see cref="IService{TEntity}.UpdateAsync(TEntity)"/>
        /// </summary>
        /// <param name="entity">Parameter <see cref="IService{TEntity}.UpdateAsync(TEntity)"/></param>
        /// <returns>Return <see cref="IService{TEntity}.UpdateAsync(TEntity)"/></returns>
        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            InitializeRepository();

            this.repository.Update(entity);
            await this.UnitOfWork.CommitAsync();

            return entity;
        }

        /// <summary>
        /// <see cref="IService{TEntity}.UpdateRangeAsync(TEntity)"/>
        /// </summary>
        /// <param name="entities">Parameter <see cref="IService{TEntity}.UpdateRangeAsync(TEntity)"/></param>
        /// <returns>Return <see cref="IService{TEntity}.UpdateRangeAsync(TEntity)"/></returns>
        public async Task<List<TEntity>> UpdateRangeAsync(List<TEntity> entities)
        {
            InitializeRepository();

            this.repository.UpdateRange(entities);
            await this.UnitOfWork.CommitAsync();

            return entities;
        }

        /// <summary>
        /// <see cref="IService{TEntity}.Delete(TEntity)"/>
        /// </summary>
        /// <param name="entity">Parameter <see cref="IService{TEntity}.Delete(TEntity)"/></param>
        public void Delete(TEntity entity)
        {
            InitializeRepository();

            this.repository.Delete(entity);
            this.UnitOfWork.Commit();
        }

        /// <summary>
        /// <see cref="IService{TEntity}.DeleteRange(TEntity)"/>
        /// </summary>
        /// <param name="entities">Parameter <see cref="IService{TEntity}.DeleteRange(TEntity)"/></param>
        public void DeleteRange(List<TEntity> entities)
        {
            InitializeRepository();

            this.repository.DeleteRange(entities);
            this.UnitOfWork.Commit();
        }

        /// <summary>
        /// <see cref="IService{TEntity}.DeleteAsync(TEntity)"/>
        /// </summary>
        /// <param name="entity">Parameter <see cref="IService{TEntity}.DeleteAsync(TEntity)"/></param>
        public async Task DeleteAsync(TEntity entity)
        {
            InitializeRepository();

            this.repository.Delete(entity);
            await this.UnitOfWork.CommitAsync();
        }

        /// <summary>
        ///  <see cref="IService{TEntity}.DeleteRangeAsync(TEntity)"/>
        /// </summary>
        /// <param name="entities">Parameter <see cref="IService{TEntity}.DeleteRangeAsync(TEntity)"/></param>
        public async Task DeleteRangeAsync(List<TEntity> entities)
        {
            InitializeRepository();

            this.repository.DeleteRange(entities);
            await this.UnitOfWork.CommitAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeRepository()
        {
            this.repository = this.UnitOfWork.Repository<TEntity>();
        }
    }
}
