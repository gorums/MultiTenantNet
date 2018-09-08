
using System;

namespace MultiTenantNet.Web.Helpers
{
    public class ApplicationConfigHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetDomain()
        {
            return System.Configuration.ConfigurationManager.AppSettings["ApplicationConfigDomain"];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetClientId()
        {
            return System.Configuration.ConfigurationManager.AppSettings["ApplicationConfigClientId"];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetClientSecret()
        {
            return System.Configuration.ConfigurationManager.AppSettings["ApplicationConfigClientSecret"];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetTenantId()
        {
            return System.Configuration.ConfigurationManager.AppSettings["ApplicationConfigTenantId"];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetSubscription()
        {
            return System.Configuration.ConfigurationManager.AppSettings["ApplicationConfigSubscription"];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal static string GetResourceGroupName()
        {
            return System.Configuration.ConfigurationManager.AppSettings["ApplicationConfigResourceGroupName"];
        }
    }
}