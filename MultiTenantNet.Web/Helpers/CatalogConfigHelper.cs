
namespace MultiTenantNet.Web.Helpers
{
    public class CatalogConfigHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetDatabase()
        {
            return System.Configuration.ConfigurationManager.AppSettings["CatalogConfigDatabase"];
        }
    }
}