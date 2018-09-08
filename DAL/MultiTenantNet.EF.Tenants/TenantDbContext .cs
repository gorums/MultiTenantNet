
namespace MultiTenantNet.EF.Tenants
{
    using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;
    using MultiTenantNet.Core;
    using MultiTenantNet.EF.Tenants.Configurations;
    using MultiTenantNet.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Migrations;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public class TenantDbContext : DbContext, IDbContext
    {
        /// <summary>
        /// An Object Context
        /// </summary>
        private ObjectContext objectContext;

        /// <summary>
        /// A transaction Object
        /// </summary>
        private DbTransaction transaction;

        /// <summary>
        /// 
        /// </summary>
        private static Dictionary<int, bool> initialize = new Dictionary<int, bool>();

        //uncomment this only when you want to add-migration on the Tenant side
        public TenantDbContext() //: base("name=TenantDb") 
        {
        }

        public TenantDbContext(ShardMap shardMap, int shardingKey, string connectionStr, string serverPaswword)
            : base(CreateDdrConnection(shardMap, shardingKey, connectionStr), true)
        {
            this.Initialize(shardingKey, serverPaswword);
        }

        public virtual DbSet<Client> Clients { get; set; }

        private static DbConnection CreateDdrConnection(ShardMap shardMap, int shardingKey, string connectionStr)
        {
            // No initialization
            Database.SetInitializer<TenantDbContext>(null);

            // Ask shard map to broker a validated connection for the given key
            SqlConnection sqlConn = shardMap.OpenConnectionForKey(shardingKey, connectionStr);

            return sqlConn;
        }

        /// <summary>
        /// <see cref="IDbContext.BeginTransaction"/>
        /// Begin an Entity Framework DataBase transaction
        /// </summary>
        public void BeginTransaction()
        {
            this.objectContext = ((IObjectContextAdapter)this).ObjectContext;
                        
            if (this.objectContext.Connection.State == ConnectionState.Open)
            {
                if (this.transaction == null)
                {
                    this.transaction = this.objectContext.Connection.BeginTransaction();
                }

                return;
            }

            this.objectContext.Connection.Open();
            this.transaction = this.objectContext.Connection.BeginTransaction();
        }

        /// <summary>
        /// <see cref="IDbContext.Commit"/>
        /// Do a commit into Entity Framework DataBase
        /// </summary>
        /// <returns>Return <see cref="IDbContext.Commit"/></returns>
        public int Commit()
        {
            try
            {
                this.BeginTransaction();
                var saveChanges = this.SaveChanges();
                this.EndTransaction();

                return saveChanges;
            }
            catch (Exception)
            {
                this.Rollback();
                throw;
            }
            finally
            {
                // base.Dispose();
            }
        }

        /// <summary>
        /// <see cref="IDbContext.CommitAsync"/>
        /// Do an async commit into Entity Framework DataBase
        /// </summary>
        /// <returns>Return <see cref="IDbContext.CommitAsync"/></returns>
        public async Task<int> CommitAsync()
        {
            try
            {
                this.BeginTransaction();
                var saveChangesAsync = await this.SaveChangesAsync();
                this.EndTransaction();

                return saveChangesAsync;
            }
            catch (Exception)
            {
                this.Rollback();
                throw;
            }
            finally
            {
                // base.Dispose();
            }
        }

        /// <summary>
        /// <see cref="IDbContext.Rollback"/>
        /// Rollback an Entity Framework transaction
        /// </summary>
        public void Rollback()
        {
            if (this.transaction?.Connection != null)
            {
                this.transaction.Rollback();
            }
        }

        /// <summary>
        /// Finish a transaction
        /// </summary>
        private void EndTransaction()
        {
            this.transaction.Commit();
            this.objectContext.Connection.Close();
            this.transaction.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public void SetAsAdded<TEntity>(TEntity entity) where TEntity : class
        {
            this.UpdateEntityState<TEntity>(entity, EntityState.Added);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public void SetAsAdded<TEntity>(List<TEntity> entities) where TEntity : class
        {
            entities.ForEach(e => this.SetAsAdded<TEntity>(e));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="trusted"></param>
        public void SetAsModified<TEntity>(TEntity entity) where TEntity : class
        {
            this.UpdateEntityState<TEntity>(entity, EntityState.Modified);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public void SetAsModified<TEntity>(List<TEntity> entities) where TEntity : class
        {
            entities.ForEach(e => this.SetAsModified<TEntity>(e));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public void SetAsDeleted<TEntity>(TEntity entity) where TEntity : class
        {
            this.UpdateEntityState<TEntity>(entity, EntityState.Deleted);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        public void SetAsDeleted<TEntity>(List<TEntity> entities) where TEntity : class
        {
            entities.ForEach(e => this.SetAsDeleted<TEntity>(e));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<TEntity> FindAsync<TEntity>(Guid id) where TEntity : class
        {
            return this.Set<TEntity>().FindAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task<TEntity> FindByCriteriaAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return this.Set<TEntity>().Local.AsQueryable().FirstOrDefaultAsync(predicate) ?? this.FirstOrDefaultAsync<TEntity>(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task<TEntity> FirstOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return this.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public Task<List<TEntity>> ToListAsync<TEntity>() where TEntity : class
        {
            return this.Set<TEntity>().ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task<List<TEntity>> ToListByCriteriaAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return this.Set<TEntity>().Where(predicate).ToListAsync<TEntity>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IQueryable<TEntity> ToQueryable<TEntity>() where TEntity : class
        {
            return this.Set<TEntity>().AsQueryable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<TEntity> ToQueryableByCriteria<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return this.Set<TEntity>().Where(predicate).AsQueryable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="entityState"></param>
        private void UpdateEntityState<TEntity>(TEntity entity, EntityState entityState) where TEntity : class
        {
            var entityEntry = this.GetDbEntityEntrySafely<TEntity>(entity);
            if (entityEntry.State == EntityState.Unchanged)
            {
                entityEntry.State = entityState;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        private DbEntityEntry GetDbEntityEntrySafely<TEntity>(TEntity entity) where TEntity : class
        {
            var entityEntry = Entry<TEntity>(entity);
            if (entityEntry.State == EntityState.Detached)
            {
                this.Set<TEntity>().Attach(entity);
            }

            return entityEntry;
        }

        /// <summary>
        /// This method run migration automatic if we need it on the tenant side
        /// </summary>
        /// <param name="shardingKey"></param>
        /// <param name="serverPaswword"></param>
        private void Initialize(int shardingKey, string serverPaswword)
        {
            SqlConnectionStringBuilder connectionString = new SqlConnectionStringBuilder(this.Database.Connection.ConnectionString)
            {
                Password = serverPaswword
            };

            if (!initialize.ContainsKey(shardingKey))
            {
                var configuration = new Configuration();
                configuration.TargetDatabase = new DbConnectionInfo(connectionString.ConnectionString, "System.Data.SqlClient");
                
                var migrator = new DbMigrator(configuration);
                migrator.Update();
            }
        }
    }
}
