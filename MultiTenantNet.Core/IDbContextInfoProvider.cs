
namespace MultiTenantNet.Core
{
    public interface IDbContextInfoProvider
    {
        string GetTenantName();

        string GetBasicSqlConnectionString();

        string GetDatabaseServerPassword();
    }
}
