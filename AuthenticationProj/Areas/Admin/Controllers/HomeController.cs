using Microsoft.AspNetCore.Mvc;

namespace ShopProj.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin")]
        public async  Task<IActionResult> Index()
        {
            return View();
        }
    }
}
