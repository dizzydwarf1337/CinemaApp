using CinemaApp.Models.Dtos;

namespace CinemaApp.Models
{
    public class Session
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public Guid MovieId { get; set; }
        public virtual Movie Movie { get; set; }
        public Guid HallId { get; set; }
        public virtual Hall Hall { get; set; }
        public double TicketPrice { get; set;}
        public ICollection<Ticket> Tickets { get; set; }
        public int AvailibleSeats { get; set; }

    }
}
