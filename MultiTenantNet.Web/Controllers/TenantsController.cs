
namespace MultiTenantNet.Web.Controllers
{
    using MultiTenantNet.EF.Catalog;
    using MultiTenantNet.Helpers;
    using MultiTenantNet.Web.Helpers;
    using System;
    using System.Linq;
    using System.Web.Mvc;

    public class TenantsController : Controller
    {
        public ActionResult Create()
        {
            return View();
        }

        // CREATE: Tenant
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                HttpContext.Server.ScriptTimeout = 600;

                var tenantName = collection.Get("tenantName");
                if (string.IsNullOrEmpty(tenantName))
                {
                    ViewBag.ErrorMsg = "Tenant name is invalid";
                    return View();
                }

                tenantName = tenantName.ToLower();

#if DEBUG
                if (!tenantName.ToLower().StartsWith("dev-"))
                {
                    ViewBag.ErrorMsg = "Tenant name need to start with dev-";
                    return View();
                }
#endif

                var db = new CatalogDbContext();
                var tenant = db.Tenants.FirstOrDefault(t => t.TenantName == tenantName);
                if (tenant == null)
                {
                    CopyDbTemplate(tenantName);
                    ShardHelper.RegisterTenantShard(DatabaseConfigHelper.GetServerNameFull(), tenantName);
                }

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
                return View();
            }
        }

        // DELETE: Tenant
        public ActionResult Delete(string tenantName)
        {
            HttpContext.Server.ScriptTimeout = 600;

            tenantName = tenantName.ToLower();

            var db = new CatalogDbContext();
            var tenant = db.Tenants.FirstOrDefault(t => t.TenantName == tenantName);
            if (tenant != null)
            {
                ShardHelper.DeleteTenantShard(DatabaseConfigHelper.GetServerNameFull(), tenantName);
                DeleteDb(tenantName);

                db.Tenants.Remove(tenant);
                db.SaveChanges();
            }

            return RedirectToAction("List");
        }

        // GET: Tenants
        public ActionResult List()
        {
            var db = new CatalogDbContext();

            ViewBag.Tenants = db.Tenants.Select(t => t.TenantName).ToList();
            ViewBag.Domain = ApplicationConfigHelper.GetDomain();

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantName"></param>
        private static void CopyDbTemplate(string tenantName)
        {
            AzureHelper.CopyNewTenantDb
            (
                TenantConfigHelper.GetTemplateDatabase(),
                tenantName,
                new Models.ApplicationConfigModel
                {
                    ClientId = ApplicationConfigHelper.GetClientId(),
                    ClientSecret = ApplicationConfigHelper.GetClientSecret(),
                    TenantId = ApplicationConfigHelper.GetTenantId(),
                    Subscription = ApplicationConfigHelper.GetSubscription(),
                    ResourceGroupName = ApplicationConfigHelper.GetResourceGroupName(),
                    ServerName = DatabaseConfigHelper.GetServerName()
                }
            );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantName"></param>
        private static void DeleteDb(string tenantName)
        {
            AzureHelper.DeleteTenantDb
            (
                TenantConfigHelper.GetTemplateDatabase(),
                tenantName,
                new Models.ApplicationConfigModel
                {
                    ClientId = ApplicationConfigHelper.GetClientId(),
                    ClientSecret = ApplicationConfigHelper.GetClientSecret(),
                    TenantId = ApplicationConfigHelper.GetTenantId(),
                    Subscription = ApplicationConfigHelper.GetSubscription(),
                    ResourceGroupName = ApplicationConfigHelper.GetResourceGroupName(),
                    ServerName = DatabaseConfigHelper.GetServerName()
                }
            );
        }
    }
}