# MultiTenantNet
Multi Tenant Dotnet App (SaaS)

## Introduction

This is a multi-tenant application prototype, one application (**SaaS**) with multiple databases, each tenant has his own database with the same schema, the use of single-tenant databases gives strong tenant isolation.

If you need differen schemas in each single-tenant database this project is easy to change using *multiples DbContext pattern* but you need to do that changes.

The framework and libraries behind are **ASP .Net MVC 5** with **.Net Framework 6.2**, **Entity Framework 6.2** and **SimpleInjector** with the idea to be hosted on Azure using **Azure SQL Elastic Database** and **Shard Map Management**. If you want to use **.Net Core** instead **.Net Framework** is not hard to do the migration I think.

### Database per tenant SaaS pattern

This pattern is effective for service providers that are concerned with tenant isolation and want to run a centralized service (SaaS) that allows cost-efficient use of shared resources. A database is created for each tenant automatic using a template database that you need to setup on your *AppSetting*. If you need to handle differet schemas you can do it using *multiples DbContext pattern*. Also they are hosted in **Azure Elastic Pools** to provide cost-efficient and easy performance management. A catalog database holds the mapping between tenants and their databases. This mapping is managed using the **shard map management** features of the **Elastic Database Client Library**, which also provides efficient connection management to the application.

![Database per Tenant](./AppVersions.PNG)

## Get started 

The first that you need to do is create an account on [Azure](https://azure.microsoft.com/en-us/), add a subscription and all the resources that you need to run this mult-tenant application and finaly setup your Web.config for you development enviroment.

```
    <!-- Database Server name where we have the template database, the catalog database and the Tenants databases Ex. myserversql, taken from myserversql.database.windows.net -->
    <add key="DatabaseConfigServerName" value="myserversql" />
    <!-- Database Server port Ex.  1433 -->
    <add key="DatabaseConfigServerPort" value="1433" />
    <!-- Database username Ex. myserversql -->
    <add key="DatabaseConfigUser" value="myserversql" />
    <!-- Database password Ex. myserversql -->
    <add key="DatabaseConfigPassword" value="myserversql" />
    <!-- Database timeout Ex. 100 -->
    <add key="DatabaseConfigConnectionTimeOut" value="100" />
    
    <!-- The template database to use as referent for the new database Tenant Ex. dev-template -->
    <add key="TenantConfigTemplateDatabase" value="dev-multtenant-template" />    
    <!-- The catalag database name Ex. dev-multtenant-catalog -->
    <add key="CatalogConfigDatabase" value="dev-multtenant-catalog" />
    
    <!-- The wildcard domain where we are going to listen all the request Ex. myapplicationmulttenant.com:58670 on development, on production we dont need the port -->
    <add key="ApplicationConfigDomain" value="myapplicationmulttenant.com:58670" />
    <!-- To know about how get clientId check https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-group-create-service-principal-portal -->
    <add key="ApplicationConfigClientId" value="5685ba91-354c-41c2-82f5-03fb156cc8aa" />
    <!-- To know about how get clientSecret check https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-group-create-service-principal-portal -->
    <add key="ApplicationConfigClientSecret" value="+gtflyb+UPPGBaQSwumJMUsSQWQufClaF3yeDb0tjZo=" />
    <!-- To know about how get tenantId check https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-group-create-service-principal-portal -->
    <add key="ApplicationConfigTenantId" value="9609f5f7-6689-4d85-8fcc-925362a06575" />
    <!-- Subscription Id -->
    <add key="ApplicationConfigSubscription" value="44439e2c-0dca-4cec-aaaf-011a850aacb2" />
    <!-- Resource group name -->
    <add key="ApplicationConfigResourceGroupName" value="EastUS" />
```

 - ConnectionString
 
 ```
 <connectionStrings>
    <add name="CatalogDb" connectionString="data source=myserversql.database.windows.net,1433;initial catalog=dev-multtenant-catalog;persist security info=True;user id=myserversql;password=myserversql;multipleactiveresultsets=True;application name=EntityFramework" providerName="System.Data.SqlClient" />
    <!--We only need to uncomment this connectionstring when we want to add-migration on the tenant side -->
    <!--add name="TenantDb" connectionString="data source=myserversql.database.windows.net,1433;initial catalog=dev-{mydb};persist security info=True;user id=myserversql;password=myserversql;multipleactiveresultsets=True;application name=EntityFramework" providerName="System.Data.SqlClient" /-->
  </connectionStrings>
 ```
### Resources on Azure

 - Subscription
 - Azure SQL Elastic Pool
 - Service Plan
 - Service Application
 - [WildCard Domain](https://azure.microsoft.com/en-us/blog/azure-websites-and-wildcard-domains/) 
 
 ### Why WildCard Domain
  
 Because we need to identify which tenant you want to hit using a subdomain Ex.
  - tenant1.myapplicationmulttenant.com (we want to hit tenant1 database)
  - tenant2.myapplicationmulttenant.com (we want to hit tenant2 database)
  
  And remember here we are creating new tenants on the fly.
  
  ```
  public class DomainHelper
    {
        public static string GetTenantName()
        {
            string[] subDomains = HttpContext.Current.Request.Url.Host.ToLower().Split('.');
            var tenantName = subDomains[0];

            return tenantName;
        }
    }
  ```
  
  ## Development Enviroment
  
  To run this project locally and test different tenants in your local machine you need to edit your host file on **C:\Windows\System32\drivers\etc** and add for example
  
  ```
  127.0.0.1       enant1.myapplicationmulttenant-test.com
  127.0.0.1       enant2.myapplicationmulttenant-test.com
  ```
  
  and then if you run the application and put on the browser **enant1.myapplicationmulttenant-test.com:58670** the application get the request instead send the request to internet. 

Notes that by default you application is listening for **localhost** not for **127.0.0.1** to change that you need to edit **.vs\config\applicationhost.config** and change
  ```
   <site name="MultiTenantNet.Web" id="2">
                <application path="/" applicationPool="Clr4IntegratedAppPool">
                    <virtualDirectory path="/" physicalPath="C:\MultiTenantNet\MultiTenantNet.Web" />
                </application>
                <bindings>
                    <binding protocol="http" bindingInformation="*:58670:localhost" />
                </bindings>
            </site>
  ```
  to
  ```
   <site name="MultiTenantNet.Web" id="2">
                <application path="/" applicationPool="Clr4IntegratedAppPool">
                    <virtualDirectory path="/" physicalPath="C:\MultiTenantNet\MultiTenantNet.Web" />
                </application>
                <bindings>
                    <binding protocol="http" bindingInformation="*:58670:127.0.0.1" />
                </bindings>
            </site>
  ```
  
  ## Lets talk about the code
  
  ...
  
