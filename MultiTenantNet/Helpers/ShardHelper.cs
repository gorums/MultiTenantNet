
namespace MultiTenantNet.Helpers
{
    using MultiTenantNet.EF.Catalog;
    using MultiTenantNet.EF.Catalog.Entitites;
    using MultiTenantNet.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;

    public class ShardHelper
    {
        /// <summary>
        /// Register tenant shard
        /// </summary>
        /// <param name="tenantServerConfig">The tenant server configuration.</param>
        /// <param name="databaseConfig">The database configuration.</param>
        /// <param name="catalogConfig">The catalog configuration.</param>
        /// <param name="resetEventDate">If set to true, the events dates for all tenants will be reset </param>
        public static void RegisterTenantShard(string tenantServer, string tenantName)
        {
            var tenantId = GetTenantKey(tenantName);
            var result = Sharding.RegisterNewShard(tenantId, tenantName, tenantServer, true);
        }

        /// <summary>
        /// Delete tenant shard
        /// </summary>
        /// <param name="tenantName">The Tenant Name.</param>
        public static void DeleteTenantShard(string tenantServer,string tenantName)
        {
            var tenantId = GetTenantKey(tenantName);
            Sharding.DeleteShard(tenantId, tenantName, tenantServer);
        }

        /// <summary>
        /// Register tenant shard
        /// </summary>
        /// <param name="tenantServerConfig">The tenant server configuration.</param>
        /// <param name="databaseConfig">The database configuration.</param>
        /// <param name="catalogConfig">The catalog configuration.</param>
        /// <param name="resetEventDate">If set to true, the events dates for all tenants will be reset </param>
        public static void RegisterAllTenantShard(string tenantServer)
        {
            var tenants = GetAllTenantNames();
            foreach (var tenant in tenants)
            {
                var tenantId = GetTenantKey(tenant.TenantName);
                var result = Sharding.RegisterNewShard(tenantId, tenant.TenantName, tenantServer);
            }
        }

        private static List<Tenants> GetAllTenantNames()
        {
            var db = new CatalogDbContext();
            return db.Tenants.ToList();            
        }

        /// <summary>
        /// Resolves any mapping differences between the global shard map in the catalog and the local shard map located a tenant database
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="UseGlobalShardMap">Specifies if the global shard map or the local shard map should be used as the source of truth for resolution.</param>
        public static void ResolveMappingDifferences(int TenantId, bool UseGlobalShardMap = false)
        {
            Sharding.ResolveMappingDifferences(TenantId, UseGlobalShardMap);
        }

        /// <summary>
        /// Gets the status of the tenant mapping in the catalog.
        /// </summary>
        /// <param name="TenantId">The tenant identifier.</param>
        public static String GetTenantStatus(int TenantId)
        {
            try
            {
                int mappingStatus = (int)Sharding.ShardMap.GetMappingForKey(TenantId).Status;
                return mappingStatus > 0 ? "Online" : "Offline";
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Converts the int key to bytes array.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static byte[] ConvertIntKeyToBytesArray(int key)
        {
            byte[] normalized = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(key));

            // Maps Int32.Min - Int32.Max to UInt32.Min - UInt32.Max.
            normalized[0] ^= 0x80;

            return normalized;
        }

        /// <summary>
        /// Generates the tenant Id using MD5 Hashing.
        /// </summary>
        /// <param name="tenantName">Name of the tenant.</param>
        /// <returns></returns>
        public static int GetTenantKey(string tenantName)
        {
            var normalizedTenantName = tenantName.Replace(" ", string.Empty).ToLower();

            //Produce utf8 encoding of tenant name 
            var tenantNameBytes = Encoding.UTF8.GetBytes(normalizedTenantName);

            //Produce the md5 hash which reduces the size
            MD5 md5 = MD5.Create();
            var tenantHashBytes = md5.ComputeHash(tenantNameBytes);

            //Convert to integer for use as the key in the catalog 
            int tenantKey = BitConverter.ToInt32(tenantHashBytes, 0);

            return tenantKey;
        }
    }
}
