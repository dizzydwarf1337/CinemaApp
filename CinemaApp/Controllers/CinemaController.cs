using CinemaApp.Db;
using CinemaApp.Models;
using CinemaApp.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Controllers
{
    public class CinemaController : BaseApiController
    {
        private readonly ApplicationContext _context;
        public CinemaController(ApplicationContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ICollection<cinemaDto>> GetCinemasAsync()
        {
            var cinemas = await _context.Cinemas.ToListAsync();
            var cinemaDtos = new List<cinemaDto>();
            foreach (var cinema in cinemas)
            {
                cinemaDtos.Add(new cinemaDto(cinema));
            }
            return cinemaDtos;

        }
        [HttpGet("{id}")]
        public async Task<cinemaDto> GetCinemaByIdAsync(Guid id)
        {
            var cinema = await _context.Cinemas.FirstOrDefaultAsync(c => c.Id == id);
            return new cinemaDto(cinema);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCinemaAsync(CreateCinemaRequest request)
        {
            string? imagePath = null;
            var cinema = new Cinema
            {
                Id = Guid.NewGuid(),
                Name = request.CinemaDto.Name,
                Address = request.CinemaDto.Address,
                ImagePath = imagePath
            };
            int numOfHalls = Convert.ToInt32(request.NumOfHalls);
            Console.WriteLine(request.NumOfHalls);
            await _context.Cinemas.AddAsync(cinema);

            for (int i = 0; i < numOfHalls; i++)
            {
                var hall = new Hall
                {
                    Id = Guid.NewGuid(),
                    CinemaId = cinema.Id,
                    Number = (i + 1).ToString(),
                    Seats = 100,
                    Cinema = cinema,
                    Sessions = new List<Session>()
                };
                await _context.Halls.AddAsync(hall);
            }

            await _context.SaveChangesAsync();
            return Ok(cinema);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UploadImageAsync(Guid id, IFormFile? formFile)
        {
            var cinema = await _context.Cinemas.FindAsync(id);
            if (cinema == null)
            {
                return NotFound("Cinema not found");
            }

            string? imagePath = null;

            if (formFile != null)
            {
                if (!string.IsNullOrEmpty(cinema.ImagePath))
                {
                    var oldImagePath = Path.Combine("client-app", "public", cinema.ImagePath.TrimStart('/'));
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
                cinema.ImagePath = imagePath;
            }

            await _context.SaveChangesAsync();
            return Ok(imagePath);
        }

        [HttpPut]
        public async Task UpdateCinemaAsync(cinemaDto cinemaDto)
        {
            var cinema = await _context.Cinemas.FirstOrDefaultAsync(c => c.Id == cinemaDto.Id);
            if (cinema == null)
            {
                throw new Exception("Cinema not found");
            }
            cinema.Name = cinemaDto.Name;
            cinema.Address = cinemaDto.Address;
            _context.Cinemas.Update(cinema);
            await _context.SaveChangesAsync();
        }
        [HttpDelete("{id}")]
        public async Task DeleteCinemaAsync(Guid id)
        {
            var cinema = await _context.Cinemas.FirstOrDefaultAsync(c => c.Id == id);
            if (cinema == null)
            {
                throw new Exception("Cinema not found");
            }

            if (!string.IsNullOrEmpty(cinema.ImagePath))
            {
                var oldImagePath = Path.Combine("client-app", "public", cinema.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            _context.Cinemas.Remove(cinema);
            await _context.SaveChangesAsync();

        }
    }
}
