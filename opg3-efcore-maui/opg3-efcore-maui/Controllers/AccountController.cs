using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using opg3_efcore_maui.Models;
using opg3_efcore_maui.Models.Requests;
using opg3_efcore_maui.Services;
using System.Security.Claims;

namespace opg3_efcore_maui.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly BookDbContext _context;
        private readonly UserManager<BookIdentitiy> _userManager;
        private readonly PasswordHasher<BookIdentitiy> _passwordHasher;
        private readonly JWTGenerator _jwtGenerator;

        public AccountController(BookDbContext dbContext, UserManager<BookIdentitiy> userManager, JWTGenerator jWTGenerator)
        {
            this._context = dbContext;
            this._userManager = userManager;
            this._passwordHasher = new PasswordHasher<BookIdentitiy>();
            this._jwtGenerator = jWTGenerator;
        }


        [HttpPost("sign-in", Name = "SignIn")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
        {

            var user = await _userManager.FindByNameAsync(request.username);

            if (user == null)
            {
                return BadRequest("User not found.");
            }


            var passwordMatch = await _userManager.CheckPasswordAsync(user, request.password);

            if(!passwordMatch)
            {
                return BadRequest("Invalid password.");
            }


            var token = await this._jwtGenerator.GenerateToken(user);

            Response.Cookies.Append("authToken", token, new CookieOptions { HttpOnly= true, Secure = true});

            return Ok(new
            {
                token,
                username = user.UserName,
            });
        }

        [HttpPost("register", Name = "Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {

            var user = await _userManager.FindByNameAsync(request.email);

            if (user != null)
            {
                return BadRequest("üsername already in use.");
            }

            var result = await _userManager.CreateAsync(new BookIdentitiy
            {
                Id= Guid.NewGuid().ToString(),
                UserName= request.username,
                Email= request.email,
                NormalizedUserName = request.username.ToUpper(),
                NormalizedEmail = request.email.ToUpper(),
            }, request.password);

            if(!result.Succeeded)
            {
                return BadRequest("Error in creating account.");
            }

            return Ok();
        }

        [HttpGet("sign-out", Name = "SignOut")]
        public IActionResult SignOut()
        {


            var token = Request.Cookies["authToken"];

            if(token != null)
            {
                Response.Cookies.Delete("authToken");
            }

            return Ok();
        }

        [Authorize]
        [HttpGet("user", Name = "User")]        
        public async Task<IActionResult> GetUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByIdAsync(userId);

            if(user == null)
            {
                return BadRequest("User not found.");
            }

            return Ok(new
            {
                id= userId,
                username = user.UserName,
                email = user.Email,
            });
        }
    }
}
