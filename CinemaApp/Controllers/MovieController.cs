using CinemaApp.Db;
using CinemaApp.Models;
using CinemaApp.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace CinemaApp.Controllers
{
    public class MovieController : BaseApiController
    {
        private readonly ApplicationContext _context;
        public MovieController(ApplicationContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ICollection<movieDto>> GetMoviesAsync()
        {
           var movies = await _context.Movies.ToListAsync();
           var moviesDtos = new List<movieDto>();
            foreach (var movie in movies)
            {
                moviesDtos.Add(new movieDto(movie));
            }
            return moviesDtos;
        }
        [HttpGet("{id}")]
        public async Task<movieDto> GetMovieByIdAsync(Guid id)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
            var moviedto= new movieDto(movie);
            return moviedto;
        }
        [HttpPost]
        public async Task CreateMovieAsync([FromBody]movieDto moviedto)
        {
            var movie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = moviedto.Title,
                Description = moviedto.Description,
                Duration = moviedto.Duration,
                Genre = moviedto.Genre,
                Director = moviedto.Director,
            };
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
        }
        [HttpPut]
        public async Task UpdateMovieAsync(movieDto moviedto)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.Id == moviedto.Id);
            movie.Title = moviedto.Title;
            movie.Description = moviedto.Description;
            movie.Duration = moviedto.Duration;
            movie.Genre = moviedto.Genre;
            movie.Director = moviedto.Director;
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
        }
        [HttpDelete("{id}")]
        public async Task DeleteMovieByIdAsync(Guid id)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }

    }
}
