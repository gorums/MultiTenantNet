
namespace MultiTenantNet.EF.Tenants.Configurations
{
    using System.Data.Entity.Migrations;

    class Configuration : DbMigrationsConfiguration<TenantDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    }
}
