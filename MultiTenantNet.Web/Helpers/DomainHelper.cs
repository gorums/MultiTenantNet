
namespace MultiTenantNet.Web.Helpers
{
    using System.Web;

    public class DomainHelper
    {
        public static string GetTenantName()
        {
            string[] subDomains = HttpContext.Current.Request.Url.Host.ToLower().Split('.');
            var tenantName = subDomains[0];

            return tenantName;
        }
    }
}