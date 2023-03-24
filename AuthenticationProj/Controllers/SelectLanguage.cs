using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationProj.Controllers
{
    public class SelectLanguage : Controller
    {
        [HttpPost]
        public IActionResult Index(string Culture ,string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(Culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

            return LocalRedirect(returnUrl);
        }
    }
}
