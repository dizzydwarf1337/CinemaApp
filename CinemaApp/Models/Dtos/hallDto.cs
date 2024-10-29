namespace CinemaApp.Models.Dtos
{
    public class hallDto
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public int Seats { get; set; }
        public Guid CinemaId { get; set; }
        public hallDto(Hall hall)
        {
            this.Id=hall.Id;
            this.Number = hall.Number;
            this.Seats= hall.Seats;
            this.CinemaId = hall.CinemaId;
        }
        public hallDto() { }
    }
}
