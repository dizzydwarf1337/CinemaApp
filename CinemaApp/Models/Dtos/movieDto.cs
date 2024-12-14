using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CinemaApp.Models.Dtos
{
    public class movieDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Duration { get; set; }
        public string ImagePath { get; set; }
        public movieDto(Movie movie) 
        {
            this.Id = movie.Id;
            this.Title = movie.Title;
            this.Description = movie.Description;
            this.Genre = movie.Genre;
            this.Director = movie.Director;
            this.ImagePath = movie.ImagePath;
            this.Duration = movie.Duration.ToString();
        }
        public movieDto() { }
    }
}
