
namespace MultiTenantNet.Helpers
{
    using Microsoft.Azure.Management.Fluent;
    using Microsoft.Azure.Management.ResourceManager.Fluent;
    using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
    using Microsoft.Azure.Management.Sql.Fluent.Models;
    using MultiTenantNet.Models;

    public class AzureHelper
    {
        /// <summary>
        /// To know about how get clientId, clientSecret and tenantId check
        /// https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-group-create-service-principal-portal
        /// </summary>
        /// <param name="templateDb"></param>
        /// <param name="newTenantDb"></param>
        /// <param name="clientId"></param>
        public static void CopyNewTenantDb(string templateDb, string newTenantDb, ApplicationConfigModel applicationModel)
        {
            var creds = new AzureCredentialsFactory().FromServicePrincipal(applicationModel.ClientId, applicationModel.ClientSecret, applicationModel.TenantId, AzureEnvironment.AzureGlobalCloud);
            IAzure azure = Azure.Authenticate(creds).WithSubscription(applicationModel.Subscription);

            var db = azure.SqlServers.Databases;
            //This retrieves the SQL server from the resourcegroup where the database needs to be created in
            var sqlServer = azure.SqlServers.GetByResourceGroup(applicationModel.ResourceGroupName, applicationModel.ServerName);

            //This database is the template database that is used for creating a new database
            var templatedb = sqlServer.Databases.Get(templateDb);

            //Create a new database based on the template database and define your name of your new database
            var database = sqlServer.Databases.Define(newTenantDb)
            .WithSourceDatabase(templatedb)
            .WithMode(CreateMode.Copy)
            .Create();
        }

        public static void DeleteTenantDb(string templateDb, string newTenantDb, ApplicationConfigModel applicationModel)
        {
            var creds = new AzureCredentialsFactory().FromServicePrincipal(applicationModel.ClientId, applicationModel.ClientSecret, applicationModel.TenantId, AzureEnvironment.AzureGlobalCloud);
            IAzure azure = Azure.Authenticate(creds).WithSubscription(applicationModel.Subscription);

            var db = azure.SqlServers.Databases;
            //This retrieves the SQL server from the resourcegroup where the database needs to be created in
            var sqlServer = azure.SqlServers.GetByResourceGroup(applicationModel.ResourceGroupName, applicationModel.ServerName);

            //This database is the template database that is used for creating a new database
            var templatedb = sqlServer.Databases.Get(templateDb);

            //Create a new database based on the template database and define your name of your new database
            sqlServer.Databases.Delete(newTenantDb);
        }
    }
}
