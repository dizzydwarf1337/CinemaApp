using CinemaApp.Db;
using CinemaApp.Models;
using CinemaApp.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Controllers
{
    public class CinemaController : BaseApiController
    {
        private readonly ApplicationContext _context;
        public CinemaController(ApplicationContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ICollection<cinemaDto>> GetCinemasAsync()
        {
            var cinemas = await _context.Cinemas.ToListAsync();
            var cinemaDtos = new List<cinemaDto>();
            foreach (var cinema in cinemas)
            {
                cinemaDtos.Add(new cinemaDto(cinema));
            }
            return cinemaDtos;

        }
        [HttpGet("{id}")]
        public async Task<cinemaDto> GetCinemaByIdAsync(Guid id)
        {
            var cinema = await _context.Cinemas.FirstOrDefaultAsync(c => c.Id == id);
            return new cinemaDto(cinema);
        }
        [HttpPost]
        public async Task CreateCinemaAsync(cinemaDto cinemaDto)
        {
            var cinema = new Cinema { Id=Guid.NewGuid(), Name = cinemaDto.Name, Address = cinemaDto.Address};
            await _context.Cinemas.AddAsync(cinema);
            await _context.SaveChangesAsync();
        }
        [HttpPut]
        public async Task UpdateCinemaAsync(cinemaDto cinemaDto)
        {
            var cinema = await _context.Cinemas.FirstOrDefaultAsync(c => c.Id == cinemaDto.Id);
            cinema.Name=cinemaDto.Name;
            cinema.Address = cinemaDto.Address;
            _context.Cinemas.Update(cinema);
            await _context.SaveChangesAsync();
        }
        [HttpDelete("{id}")]
        public async Task DeleteCinemaAsync(Guid id)
        {
            var cinema = await _context.Cinemas.FirstOrDefaultAsync(c => c.Id == id);
            _context.Cinemas.Remove(cinema);
            await _context.SaveChangesAsync();
        }
    }
}
