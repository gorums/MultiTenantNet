
namespace MultiTenantNet.EF.Catalog.Entitites
{
    using System;

    public class Tenants
    {
        public byte[] TenantId { get; set; }
        public string TenantName { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
