namespace CinemaApp.Models.Dtos
{
    public class cinemaDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public cinemaDto (Cinema cinema)
        {
            this.Id= cinema.Id;
            this.Name= cinema.Name;
            this.Address= cinema.Address;
        }
        public cinemaDto() { }
    }
}
