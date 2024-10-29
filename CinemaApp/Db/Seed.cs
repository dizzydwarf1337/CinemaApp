using CinemaApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Db
{
    public static class Seed
    {
        public static async void SeedData(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApplicationContext>>());

            if (context.Users.Any())
            {
                return;
            }
            else
            {
                await context.Users.AddRangeAsync
                    (
                    new User { Id=Guid.NewGuid(),Email = "admin@gmail.com", Password = "admin", Role = "admin", Name = "Admin", LastName = "Admin" },
                    new User { Id = Guid.NewGuid(), Email = "user@gmail.com", Password = "user", Role = "user", Name = "User", LastName = "User" }
                    );
                await context.SaveChangesAsync();
                await context.Cinemas.AddAsync(
                    new Cinema {Id=Guid.NewGuid(), Name="Cinema1", Address="City1, Street1, House1", }
                    );
                await context.SaveChangesAsync();
                await context.Halls.AddAsync(
                    new Hall { Id=Guid.NewGuid(), Number="1", Seats=100, CinemaId = context.Cinemas.First().Id }
                    );
                await context.SaveChangesAsync();
                await context.Movies.AddRangeAsync(
                    new Movie { Id=Guid.NewGuid(), Title = "Movie1", Description = "Description1", Genre = "Genre1", Director = "Director1" },
                    new Movie { Id = Guid.NewGuid(), Title = "Movie2", Description = "Description2", Genre = "Genre2", Director = "Director2" }
                    );
                await context.SaveChangesAsync();
                await context.Sessions.AddRangeAsync(
                    new Session { Id = Guid.NewGuid(), MovieId = context.Movies.FirstOrDefault(x=>x.Title=="Movie1").Id, HallId = context.Halls.First().Id, Date = DateTime.Now.AddDays(1), AvailibleSeats = context.Halls.First().Seats },
                    new Session { Date = DateTime.Now.AddDays(2), MovieId = context.Movies.FirstOrDefault(x => x.Title == "Movie2").Id, HallId = context.Halls.First().Id, Id = Guid.NewGuid(), AvailibleSeats = context.Halls.First().Seats }
                    );
                await context.SaveChangesAsync();
            }
        }
    }
}
