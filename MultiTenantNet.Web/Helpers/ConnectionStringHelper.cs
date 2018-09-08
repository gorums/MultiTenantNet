
namespace MultiTenantNet.Web.Helpers
{
    using System.Data.SqlClient;

    public class ConnectionStringHelper
    {
        /// <summary>
        /// Gets the basic SQL connection string.
        /// </summary>
        /// <returns></returns>
        public static string GetBasicSqlConnectionString()
        {
            var connStrBldr = new SqlConnectionStringBuilder
            {
                UserID = DatabaseConfigHelper.GetUse(),
                Password = DatabaseConfigHelper.GetPassword(),
                ApplicationName = "EntityFramework",
                ConnectTimeout = int.Parse(DatabaseConfigHelper.GetConnectionTimeOut()),
                LoadBalanceTimeout = 15,
            };

            return connStrBldr.ConnectionString;
        }
    }
}