namespace CinemaApp.Models
{
    public class Hall
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public int Seats { get; set; }
        public Guid CinemaId { get; set; }
        public virtual Cinema Cinema { get; set; }
        public ICollection<Session> Sessions { get; set; }
    }
}
