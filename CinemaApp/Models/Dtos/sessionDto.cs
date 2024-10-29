namespace CinemaApp.Models.Dtos
{
    public class sessionDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public Guid MovieId { get; set; }
        public Guid HallId { get; set; }
        public double TicketPrice { get; set; }
        public int AvailibleSeats { get; set; }
        public sessionDto(Session session)
        {
            Id = session.Id;
            Date = session.Date;
            MovieId = session.MovieId;
            HallId = session.HallId;
            TicketPrice = session.TicketPrice;
            AvailibleSeats = session.AvailibleSeats;
        }
        public sessionDto() { }
    }
}
