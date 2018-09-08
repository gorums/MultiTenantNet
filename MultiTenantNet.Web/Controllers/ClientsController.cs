
namespace MultiTenantNet.Web.Controllers
{
    using Bogus;
    using MultiTenantNet.Core.Services;
    using MultiTenantNet.Entities;
    using MultiTenantNet.Web.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    public class ClientsController : Controller
    {
        private readonly IClientService clientService;

        public ClientsController(IClientService clientService)
        {
            this.clientService = clientService;
        }

        // GET: Clients
        public async Task<ActionResult> Index()
        {         
            var clients = await this.clientService.GetAllAsync();
            if (clients == null || clients.Count == 0)
            {
                clients = await this.CreateClients();
            }

            ViewBag.TenantName = DomainHelper.GetTenantName();
            ViewBag.Clients = clients;

            return View();
        }

        private async Task<List<Client>> CreateClients()
        {
            await this.clientService.AddRangeAsync
            (
                new Faker<Client>("en")
                    .RuleFor(r => r.Id, f => Guid.NewGuid())
                    .RuleFor(r => r.Username, f => f.Person.UserName)
                    .RuleFor(r => r.FirstName, f => f.Person.FirstName)
                    .RuleFor(r => r.LastName, f => f.Person.LastName)
                    .RuleFor(r => r.Email, f => f.Person.Email)

                .Generate(new Random(Environment.TickCount).Next(3, 15))
            );

            return await this.clientService.GetAllAsync();
        }        
    }
}
