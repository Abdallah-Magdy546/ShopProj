using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ShopProj.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.UserId = UserId;
            return View();
        }
    }
}
