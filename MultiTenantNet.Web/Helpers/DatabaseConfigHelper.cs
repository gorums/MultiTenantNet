
namespace MultiTenantNet.Web.Helpers
{
    public class DatabaseConfigHelper
    {
        /// <summary>
        /// Gets database server name.
        /// </summary>
        /// <returns></returns>
        public static string GetServerName()
        {
            return System.Configuration.ConfigurationManager.AppSettings["DatabaseConfigServerName"];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetServerNameFull()
        {
            return System.Configuration.ConfigurationManager.AppSettings["DatabaseConfigServerName"] + ".database.windows.net";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetPassword()
        {
            return System.Configuration.ConfigurationManager.AppSettings["DatabaseConfigPassword"];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetUse()
        {
            return System.Configuration.ConfigurationManager.AppSettings["DatabaseConfigUser"];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionTimeOut()
        {
            return System.Configuration.ConfigurationManager.AppSettings["DatabaseConfigConnectionTimeOut"];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetServerPort()
        {
            return System.Configuration.ConfigurationManager.AppSettings["DatabaseConfigServerPort"];
        }
    }
}