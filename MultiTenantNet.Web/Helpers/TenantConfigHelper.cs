using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MultiTenantNet.Web.Helpers
{
    public class TenantConfigHelper
    {
        public static string GetTemplateDatabase()
        {
            return System.Configuration.ConfigurationManager.AppSettings["TenantConfigTemplateDatabase"];
        }
    }
}