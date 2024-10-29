using CinemaApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Db
{
    public class ApplicationContext : DbContext
    {
        
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options){}
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
