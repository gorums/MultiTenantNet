
namespace MultiTenantNet
{
    using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;
    using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement.Recovery;
    using MultiTenantNet.EF.Catalog;
    using MultiTenantNet.EF.Catalog.Entitites;
    using MultiTenantNet.Helpers;
    using System;
    using System.Diagnostics;

    public class Sharding
    {
        public ShardMapManager ShardMapManager { get; }

        public static ListShardMap<int> ShardMap { get; set; }

        private static String shardMapConnectionString;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="catalogDatabase"></param>
        /// <param name="connectionString"></param>
        public Sharding(string catalogDatabase, string connectionString)
        {
            try
            {
                shardMapConnectionString = connectionString;

                // Deploy shard map manager
                // if shard map manager exists, refresh content, else create it, then add content                
                ShardMapManager =
                    !ShardMapManagerFactory.TryGetSqlShardMapManager(connectionString,
                        ShardMapManagerLoadPolicy.Lazy, out ShardMapManager smm)
                        ? ShardMapManagerFactory.CreateSqlShardMapManager(connectionString)
                        : smm;

                // check if shard map exists and if not, create it 
                ShardMap = !ShardMapManager.TryGetListShardMap(catalogDatabase, out ListShardMap<int>  sm)
                    ? ShardMapManager.CreateListShardMap<int>(catalogDatabase)
                    : sm;
            }
            catch (Exception exception)
            {
                Trace.TraceError(exception.Message, "Error in sharding initialisation.");
            }
        }

        /// <summary>
        /// Registers the new shard.
        /// Verify if shard exists for the tenant. If not then create new shard and add tenant details to Tenants table in catalog
        /// </summary>
        /// <param name="tenantName">Name of the tenant.</param>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="tenantServer">The tenant server.</param>
        /// <returns></returns>
        public static bool RegisterNewShard(int tenantId, string tenantName, string tenantServer, bool needToSaveOnDB = false)
        {
            try
            {
                ShardLocation shardLocation = new ShardLocation(tenantServer, tenantName);

                if (!ShardMap.TryGetShard(shardLocation, out Shard shard))
                {
                    //create shard if it does not exist
                    shard = ShardMap.CreateShard(shardLocation);
                }

                // Register the mapping of the tenant to the shard in the shard map.
                // After this step, DDR on the shard map can be used
                if (!ShardMap.TryGetMappingForKey(tenantId, out PointMapping<int>  mapping))
                {
                    var pointMapping = ShardMap.CreatePointMapping(tenantId, shard);

                    if (needToSaveOnDB)
                    {
                        //convert from int to byte[] as tenantId has been set as byte[] in Tenants entity
                        var key = ShardHelper.ConvertIntKeyToBytesArray(pointMapping.Value);

                        //add tenant to Tenants table
                        var tenant = new Tenants
                        {
                            TenantId = key,
                            TenantName = tenantName,
                            LastUpdated = DateTime.Now
                        };

                        var db = new CatalogDbContext();
                        db.Tenants.Add(tenant);
                        db.SaveChanges();
                    }
                }

                return true;
            }
            catch (Exception exception)
            {
                Trace.TraceError(exception.Message, "Error in registering new shard.");
                return false;
            }

        }

        /// <summary>
        /// Resolves any mapping differences between the global shard map in the catalog and the local shard map located a tenant database
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="UseGlobalShardMap">Specifies if the global shard map or the local shard map should be used as the source of truth for resolution.</param>       
        public static void ResolveMappingDifferences(int TenantId, bool UseGlobalShardMap = false)
        {
            var shardMapManager = ShardMapManagerFactory.GetSqlShardMapManager(shardMapConnectionString, ShardMapManagerLoadPolicy.Lazy);
            var recoveryManager = shardMapManager.GetRecoveryManager();
            var tenantMapping = ShardMap.GetMappingForKey(TenantId);
            var mappingDifferencesList = recoveryManager.DetectMappingDifferences(tenantMapping.Shard.Location, ShardMap.Name);

            foreach (var mismatch in mappingDifferencesList)
            {
                if (!UseGlobalShardMap)
                {
                    recoveryManager.ResolveMappingDifferences(mismatch, MappingDifferenceResolution.KeepShardMapping);
                }
                else
                {
                    recoveryManager.ResolveMappingDifferences(mismatch, MappingDifferenceResolution.KeepShardMapMapping);
                }
            }

        }

        /// <summary>
        /// Delete a Shard from ShardMap and ShardMapGlobal
        /// </summary>
        /// <param name="tenantName"></param>
        /// <param name="tenantServer"></param>
        public static void DeleteShard(int tenantId, string tenantName, string tenantServer)
        {
            try
            {
                if (ShardMap.TryGetMappingForKey(tenantId, out PointMapping<int> mapping))
                {
                    if (mapping.Status == MappingStatus.Online)
                    {
                        mapping = ShardMap.MarkMappingOffline(mapping);
                    }

                    ShardMap.DeleteMapping(mapping);

                    ShardLocation shardLocation = new ShardLocation(tenantServer, tenantName);
                    if (ShardMap.TryGetShard(shardLocation, out Shard shard))
                    {
                        ShardMap.DeleteShard(shard);
                    }
                }                    
            }
            catch (Exception exception)
            {
                Trace.TraceError(exception.Message, "Error in registering new shard.");
            }
        }
    }
}
