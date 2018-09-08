
namespace MultiTenantNet
{
    using MultiTenantNet.Core;
    using MultiTenantNet.EF.Tenants;
    using MultiTenantNet.Helpers;
    using MultiTenantNet.Models;
    using System;
    using System.Collections;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// The DataBase Context
        /// </summary>
        private IDbContext context;

        private readonly IDbContextInfoProvider contextInfoProvider;
        /// <summary>
        /// A hash of repositories
        /// </summary>
        private Hashtable repositories;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork" /> class
        /// </summary>
        public UnitOfWork(IDbContextInfoProvider contextInfoProvider)
        {
            this.contextInfoProvider = contextInfoProvider;
        }

        /// <summary>
        /// <see cref="IUnitOfWork.Repository{TEntity}"/>
        /// </summary>
        /// <typeparam name="TEntity">The Entity</typeparam>
        /// <returns>Return <see cref="IUnitOfWork.Repository{TEntity}"/></returns>
        public IRepository<TEntity> Repository<TEntity>()
            where TEntity : class
        {
            InitializeContext();

            if (this.repositories == null)
            {
                this.repositories = new Hashtable();
            }

            var type = typeof(TEntity).Name;

            if (this.repositories.ContainsKey(type))
            {
                return (IRepository<TEntity>)this.repositories[type];
            }

            var repositoryType = typeof(Repository<TEntity>);

            this.repositories.Add(type, Activator.CreateInstance(repositoryType, this.context));

            return (IRepository<TEntity>)this.repositories[type];
        }        

        /// <summary>
        /// <see cref="IUnitOfWork.BeginTransaction"/>
        /// </summary>
        public void BeginTransaction()
        {
            InitializeContext();

            this.context.BeginTransaction();
        }

        /// <summary>
        /// <see cref="IUnitOfWork.Commit"/>
        /// </summary>
        /// <returns>Return <see cref="IUnitOfWork.Commit"/></returns>
        public int Commit()
        {
            InitializeContext();

            return this.context.Commit();
        }

        /// <summary>
        /// <see cref="IUnitOfWork.CommitAsync"/>
        /// </summary>
        /// <returns>Return <see cref="IUnitOfWork.CommitAsync"/></returns>
        public Task<int> CommitAsync()
        {
            InitializeContext();

            return this.context.CommitAsync();
        }

        /// <summary>
        /// <see cref="IUnitOfWork.Rollback"/>
        /// </summary>
        public void Rollback()
        {
            InitializeContext();

            this.context.Rollback();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeContext()
        {
            if (this.context == null)
            {
                // tenantId base on the name
                var tenantId = ShardHelper.GetTenantKey(this.contextInfoProvider.GetTenantName());

                // database parameters
                var basicConnectionString = this.contextInfoProvider.GetBasicSqlConnectionString();
                var databaseServerPassword = this.contextInfoProvider.GetDatabaseServerPassword();

                this.context = new TenantDbContext(Sharding.ShardMap, tenantId, basicConnectionString, databaseServerPassword);
            }
        }
    }
}
