using CinemaApp.Models.Dtos;

public class CreateCinemaRequest
{
    public cinemaDto CinemaDto { get; set; }
    public int NumOfHalls { get; set; }
}