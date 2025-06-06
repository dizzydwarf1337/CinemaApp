﻿using CinemaApp.Db;
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
        public async Task<IActionResult> CreateMovieAsync(movieDto movieDto)
        {
            string? imagePath = null;
            var movie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = movieDto.Title,
                Description = movieDto.Description,
                Duration = TimeOnly.Parse(movieDto.Duration),
                Genre = movieDto.Genre,
                Director = movieDto.Director,
                ImagePath = imagePath 
            };


            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
            return Ok(movie);
        }
        [HttpPut("{id}")]
        public async Task UploadImage(Guid id, IFormFile? formFile)
        {
            var movie = await _context.Movies.FindAsync(id);

            string? imagePath = null;

            if (formFile != null)
            {
                if (!string.IsNullOrEmpty(movie.ImagePath))
                {
                    var oldImagePath = Path.Combine("client-app", "public", movie.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                var fileName = $"{Guid.NewGuid()}_{formFile.FileName}";
                var directoryPath = Path.Combine("client-app", "public", "images");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var filePath = Path.Combine(directoryPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                imagePath = $"/images/{fileName}";
                movie.ImagePath = imagePath;
            }
            await _context.SaveChangesAsync();

        }
        [HttpPut]
        public async Task UpdateMovieAsync(movieDto movieDto)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == movieDto.Id);
            if (movie == null)
            {
                throw new Exception("Movie not found");
            }

            movie.Title = movieDto.Title;
            movie.Description = movieDto.Description;
            movie.Duration = TimeOnly.Parse(movieDto.Duration);
            movie.Genre = movieDto.Genre;
            movie.Director = movieDto.Director;
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
        }
        [HttpDelete("{id}")]
        public async Task DeleteMovieByIdAsync(Guid id)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                throw new Exception("Movie not found");
            }

            if (!string.IsNullOrEmpty(movie.ImagePath))
            {
                var oldImagePath = Path.Combine("client-app", "public", movie.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }


    }
}
