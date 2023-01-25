using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using opg2_login.Models;
using System.Collections.Generic;

namespace opg2_login.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly List<User> _users = new();

        public AuthController() {
            _users.Add(new User()
            {
                Username = "admin",
                Password = "Password1!"
            }); ;
        }

        [HttpPost("Login", Name ="Login")]
        public ActionResult Login([FromForm] string username, [FromForm] string password)
        {
            var foundUser = _users.Find(u => u.Username.ToLower() == username.ToLower());

            if(foundUser == null)
            {

                return RedirectToAction("index","Home", new
                {
                    error = "Bruger ikke fundet."

                });
            }

            if(foundUser.Password != password)
            {
                return RedirectToPage("/", new
                {
                    error = "Adgangskoden er forkert."
                });
            }

            Response.Cookies.Append("username", foundUser.Username);

            return Redirect("/home/profile");
        }
    }
}
