namespace CinemaApp.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public virtual Session Session { get; set; }
        public string Seat { get; set; }
        public string Status { get; set; }
        public double Price { get; set; }
        public virtual DateTime Created { get; set; } = DateTime.Now;
        public int NumberOfSeats { get; set; }
        public Guid UserId { get; set;}
        public User user { get; set; }
    }
}
