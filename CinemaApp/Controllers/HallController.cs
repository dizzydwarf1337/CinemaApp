using CinemaApp.Db;
using CinemaApp.Models;
using CinemaApp.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Controllers
{
    public class HallController : BaseApiController
    {
        private readonly ApplicationContext _context;
        public HallController(ApplicationContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ICollection<hallDto>> GetHallsAsync()
        {
            var halls = await _context.Halls.ToListAsync();
            var hallsDto = new List<hallDto>();
            foreach (var hall in halls)
            {
                hallsDto.Add(new hallDto(hall));
            }
            return hallsDto;
        }
        [HttpGet("{id}")]
        public async Task<hallDto> GetHallByIdAsync(Guid id)
        {
            return new hallDto( await _context.Halls.FirstOrDefaultAsync(h => h.Id == id));
        }
        [HttpPost]
        public async Task CreateHallAsync(hallDto halldto)
        {
            var hall = new Hall { Id = Guid.NewGuid(), Seats=halldto.Seats,Number = halldto.Number, CinemaId = halldto.CinemaId };
            await _context.Halls.AddAsync(hall);
            await _context.SaveChangesAsync();
        }
        [HttpPut]
        public async Task UpdateHallAsync(hallDto halldto)
        {
            var hall = await _context.Halls.FirstOrDefaultAsync(h => h.Id == halldto.Id);
            hall.Seats = halldto.Seats;
            hall.Number = halldto.Number;
            hall.CinemaId = halldto.CinemaId;
            _context.Halls.Update(hall);
            await _context.SaveChangesAsync();
        }
        [HttpDelete("{id}")]
        public async Task DeleteHallAsync(Guid id)
        {
            var hall = await _context.Halls.FirstOrDefaultAsync(h => h.Id == id);
            _context.Halls.Remove(hall);
            await _context.SaveChangesAsync();
        }
    }
}
