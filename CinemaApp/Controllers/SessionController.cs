using CinemaApp.Db;
using CinemaApp.Models;
using CinemaApp.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Controllers
{
    public class SessionController : BaseApiController
    {
        private readonly ApplicationContext _context;
        public SessionController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ICollection<sessionDto>> GetSessionAsync()
        {
            var sessions = await _context.Sessions.ToListAsync();
            var sessionDtos = new List<sessionDto>();
            foreach (var session in sessions)
            {
                sessionDtos.Add(new sessionDto(session));
            }
            return sessionDtos;
        }
        [HttpGet("{id}")]
        public async Task<sessionDto> GetSessionByIdAsync(Guid id)
        {
            return new sessionDto(await _context.Sessions.FirstOrDefaultAsync(s => s.Id == id));
        }
        [HttpPost]
        public async Task CreateSessionAsync(sessionDto sessiondto)
        {
            var session = new Session
            {
                Id = Guid.NewGuid(),
                MovieId = sessiondto.MovieId,
                HallId = sessiondto.HallId,
                Date = sessiondto.Date,
                TicketPrice = sessiondto.TicketPrice,
                AvailibleSeats = sessiondto.AvailibleSeats
            };
            await _context.Sessions.AddAsync(session);
            await _context.SaveChangesAsync();
        }
        [HttpPut]
        public async Task UpdateSessionAsync(sessionDto sessiondto)
        {
            var session = await _context.Sessions.FirstOrDefaultAsync(s => s.Id == sessiondto.Id);
            session.MovieId = sessiondto.MovieId;
            session.HallId = sessiondto.HallId;
            session.Date = sessiondto.Date;
            session.TicketPrice = sessiondto.TicketPrice;
            session.AvailibleSeats = sessiondto.AvailibleSeats;
            _context.Sessions.Update(session);
            await _context.SaveChangesAsync();
        }
        [HttpDelete("{id}")]
        public async Task DeleteSessionByIdAsync(Guid id)
        {
            var session = await _context.Sessions.FirstOrDefaultAsync(s => s.Id == id);
            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();
        }

    }
}
