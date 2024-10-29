namespace CinemaApp.Models.Dtos
{
    public class ticketDto
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public string Seat { get; set; }
        public string Status { get; set; }
        public double Price { get; set; }
        public virtual DateTime Created { get; set; } = DateTime.Now;
        public int NumberOfSeats { get; set; }
        public Guid UserId { get; set; }
        public ticketDto(Ticket ticket)
        {
            Id = ticket.Id;
            SessionId = ticket.SessionId;
            Seat = ticket.Seat;
            Status = ticket.Status;
            Price = ticket.Price;
            Created = ticket.Created;
            NumberOfSeats = ticket.NumberOfSeats;
            UserId = ticket.UserId;
        }
        public ticketDto() { }
    }
}
