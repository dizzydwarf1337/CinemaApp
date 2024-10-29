namespace CinemaApp.Models.Dtos
{
    public class userDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public userDto() { }
        public userDto (User user)
        {
            this.Id = user.Id;
            this.Email = user.Email;
            this.Password = user.Password;
            this.Role = user.Role;
            this.Name = user.Name;
            this.LastName = user.LastName;
        }
    }
}
