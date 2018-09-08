
namespace MultiTenantNet.Models
{
    public class ApplicationConfigModel
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string TenantId { get; set; }

        public string Subscription { get; set; }

        public string ResourceGroupName { get; set; }

        public string ServerName { get; set; }
    }
}
