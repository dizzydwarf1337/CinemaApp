using CinemaApp.Db;
using CinemaApp.Models;
using CinemaApp.Models.Dtos;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Controllers
{
    public class LoginController : BaseApiController
    {
        private readonly ApplicationContext _context;
        public LoginController(ApplicationContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == login.Email);
            if ( user!=null && user.Password==login.Password)
            {
                var userResponse = new userDto(user);
                return Ok(new { message = "Login successful", User = userResponse});
            }

            return Unauthorized("Invalid credentials");
        }
    }
}
