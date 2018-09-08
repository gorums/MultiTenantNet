
namespace MultiTenantNet.Controllers
{
    using MultiTenantNet.Web.Helpers;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.TenantName = DomainHelper.GetTenantName();

            return View();
        }
    }
}
