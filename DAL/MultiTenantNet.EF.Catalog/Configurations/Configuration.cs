
namespace MultiTenantNet.EF.Catalog.Configurations
{
    using System.Data.Entity.Migrations;

    class Configuration : DbMigrationsConfiguration<CatalogDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    }
}
