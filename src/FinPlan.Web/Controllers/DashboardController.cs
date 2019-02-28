using Microsoft.AspNetCore.Mvc;

namespace FinPlan.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}