using CinemaApp.Db;
using CinemaApp.Models;
using CinemaApp.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly ApplicationContext _context;
        public UserController(ApplicationContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ICollection<userDto>> GetUserAsync()
        {
            var users = await _context.Users.ToListAsync();
            var usersDtos = new List<userDto>();
            foreach (var user in users)
            {
                usersDtos.Add(new userDto(user));
            }
            return usersDtos;
        }
        [HttpGet("{id}")]
        public async Task<userDto> GetUserByIdAsync(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return new userDto(user);
        }
        [HttpPost]
        public async Task CreateUserAsync(userDto userdto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = userdto.Email,
                Password = userdto.Password,
                Role = "user",
                Name = userdto.Name,
                LastName = userdto.LastName
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        [HttpPut]
        public async Task UpdateUserAsync(userDto userdto)
        {
           var user = _context.Users.FirstOrDefault(u => u.Id == userdto.Id);
            user.Email = userdto.Email;
            user.Password = userdto.Password;
            user.Role = "user";
            user.Name = userdto.Name;
            user.LastName = userdto.LastName;
            
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        [HttpDelete("{id}")]
        public async Task DeleteUserByIdAsync(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
