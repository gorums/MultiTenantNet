
namespace MultiTenantNet.Web
{
    using MultiTenantNet.Core;
    using MultiTenantNet.Web.Helpers;

    public class DbContextInfoProvider : IDbContextInfoProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetBasicSqlConnectionString()
        {
            return ConnectionStringHelper.GetBasicSqlConnectionString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetDatabaseServerPassword()
        {
            return DatabaseConfigHelper.GetPassword();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetTenantName()
        {
            return DomainHelper.GetTenantName();
        }
    }
}