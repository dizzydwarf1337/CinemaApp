namespace CinemaApp.Models
{
    public class Movie
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public TimeOnly Duration { get; set; }
        public string? ImagePath { get; set; }
        public ICollection<Session> Sessions { get; set; }
    }
}
