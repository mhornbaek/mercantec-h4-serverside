using Microsoft.AspNetCore.Mvc;

namespace opg2_login.Controllers
{
    public class HomeController : Controller
    {


        public IActionResult Index([FromQuery] string? error)
        {   
            return View("index", error);
        }

        public IActionResult Profile()
        {
            var username = Request.Cookies["username"];

            if (username == null)
                return Redirect("/");

            return View("profile");
        }

    }
}
