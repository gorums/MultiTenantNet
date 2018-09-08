namespace MultiTenantNet.EF.Catalog.Configurations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTenantTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tenants",
                c => new
                    {
                        TenantId = c.Binary(nullable: false, maxLength: 128),
                        TenantName = c.String(),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TenantId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tenants");
        }
    }
}
