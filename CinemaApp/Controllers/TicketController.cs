using CinemaApp.Db;
using CinemaApp.Models;
using CinemaApp.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Controllers
{
    public class TicketController : BaseApiController
    {
        private readonly ApplicationContext _context;
        public TicketController(ApplicationContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ICollection<ticketDto>> GetTicketAsync()
        {
            var tickets = await _context.Tickets.ToListAsync();
            var tiketsDtos = new List<ticketDto>();
            foreach (var ticket in tickets)
            {
                tiketsDtos.Add(new ticketDto(ticket));
            }
            return tiketsDtos;
        }
        [HttpGet("{id}")]
        public async Task<ticketDto> GetTicketByIdAsync(Guid id)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);
            return new ticketDto(ticket);
        }
        [HttpGet("user/{id}")]
            public async Task<ICollection<ticketDto>> GetTicketsByUserIdAsync(Guid id)
            { 
                var tickets = await _context.Tickets.Where(x => x.UserId == id).ToListAsync();
                var ticketDtos = new List<ticketDto>();
                try
                {

                foreach (var ticket in tickets)
                {
                    ticketDtos.Add(new ticketDto(ticket));
                }
                return ticketDtos;
                }
                catch (Exception e)
                {
                Console.WriteLine(tickets);
                return null;
                }
        }
        [HttpPost]
        public async Task<IActionResult> CreateTicketAsync(ticketDto ticketdto)
        {
            var ticket = new Ticket
            {
                Id = Guid.NewGuid(),
                SessionId = ticketdto.SessionId,
                Seat = ticketdto.Seat,
                Price = ticketdto.Price,
                Status = ticketdto.Status,
                Created = ticketdto.Created,
                NumberOfSeats = ticketdto.NumberOfSeats,
                UserId = ticketdto.UserId
            };
            var session = await _context.Sessions.FirstOrDefaultAsync(s => s.Id == ticketdto.SessionId);
            session.AvailibleSeats -= ticketdto.NumberOfSeats;
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
            return Ok(ticket.Id);
        }
        [HttpPut]
        public async Task UpdateTicketAsync(ticketDto ticketdto)
        {
            var ticket = await _context.Tickets.FindAsync(ticketdto.Id);
            ticket.SessionId = ticketdto.SessionId;
            ticket.Seat = ticketdto.Seat;
            ticket.Price = ticketdto.Price;
            ticket.Status = ticketdto.Status;
            ticket.Created = ticketdto.Created;
            ticket.NumberOfSeats = ticketdto.NumberOfSeats;
            ticket.UserId = ticketdto.UserId;
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
        }
        [HttpDelete("{id}")]
        public async Task DeleteTicketByIdAsync(Guid id)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
        }
        [HttpGet("/session/${sessionId}")]
        public async Task<List<Ticket>> GetTicketsBySessionIdAsync(Guid sessionId)
        {
            return await _context.Tickets
                .Where(t => t.SessionId == sessionId)
                .ToListAsync();
        }

    }
}
