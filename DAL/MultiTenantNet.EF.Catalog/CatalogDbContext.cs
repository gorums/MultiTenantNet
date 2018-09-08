
namespace MultiTenantNet.EF.Catalog
{
    using MultiTenantNet.EF.Catalog.Entitites;
    using System.Data.Entity;

    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext() :
          base("name=CatalogDb")
        {
        }

        public virtual DbSet<Tenants> Tenants { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tenants>()
                .HasKey(e => e.TenantId);
        }
    }
}
