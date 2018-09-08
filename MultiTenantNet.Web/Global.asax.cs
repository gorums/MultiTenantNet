using MultiTenantNet.Core;
using MultiTenantNet.Core.Services;
using MultiTenantNet.Helpers;
using MultiTenantNet.Services;
using MultiTenantNet.Web;
using MultiTenantNet.Web.Helpers;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Lifestyles;
using System.Data.SqlClient;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MultiTenantNet
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var container = new Container();

            var mixed = Lifestyle.CreateHybrid(new AsyncScopedLifestyle(), new ThreadScopedLifestyle());
            var style = Lifestyle.CreateHybrid(new WebRequestLifestyle(), mixed);

            container.Options.DefaultScopedLifestyle = style;

            container.Register<IDbContextInfoProvider, DbContextInfoProvider>(Lifestyle.Scoped);
            container.Register<IUnitOfWork, UnitOfWork>(Lifestyle.Scoped);
            container.Register<IClientService, ClientService>(Lifestyle.Scoped);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.Verify();
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            InitialiseShardMapManager();
            ShardHelper.RegisterAllTenantShard(DatabaseConfigHelper.GetServerNameFull());            
        }

        /// <summary>
        /// Initialises the shard map manager and shard map 
        /// <para>Also does all tasks related to sharding</para>
        /// </summary>
        private void InitialiseShardMapManager()
        {
            var basicConnectionString = ConnectionStringHelper.GetBasicSqlConnectionString();
            SqlConnectionStringBuilder connectionString = new SqlConnectionStringBuilder(basicConnectionString)
            {
                DataSource = "tcp:" + DatabaseConfigHelper.GetServerNameFull() + "," + DatabaseConfigHelper.GetServerPort(),
                InitialCatalog = CatalogConfigHelper.GetDatabase()
            };

            var sharding = new Sharding(CatalogConfigHelper.GetDatabase(), connectionString.ConnectionString);
        }       

    }
}
