
namespace MultiTenantNet.Services
{
    using MultiTenantNet.Core;
    using MultiTenantNet.Core.Services;
    using MultiTenantNet.Entities;

    public class ClientService : Service<Client>, IClientService, IService<Client>
    {
        public ClientService(IUnitOfWork unitOfWork) : 
            base(unitOfWork)
        {
        }
    }
}
