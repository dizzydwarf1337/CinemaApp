namespace CinemaApp.Models
{
    public class Cinema
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public virtual ICollection<Hall> Halls { get; set; }
    }
}
