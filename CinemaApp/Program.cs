using CinemaApp.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationContext>(opt =>

    opt.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CinemaApp;Trusted_Connection=True;MultipleActiveResultSets=true")
);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{

}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        Seed.SeedData(services);
    }
    catch (Exception ex)
    {
        // Логирование ошибок
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
