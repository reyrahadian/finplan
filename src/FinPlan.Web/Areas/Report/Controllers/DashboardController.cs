using Microsoft.AspNetCore.Mvc;

namespace FinPlan.Web.Areas.Report.Controllers
{
    [Area("Report")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}