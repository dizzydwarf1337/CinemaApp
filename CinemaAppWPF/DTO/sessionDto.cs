using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CinemaAppWPF.DTO
{
    public class sessionDto
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public Guid HallId { get; set; }
        public DateTime Date { get; set; }
        public decimal TicketPrice { get; set; }
        public int AvailibleSeats { get; set; }
        [JsonIgnore]
        public string MovieTitle { get; set; }

        [JsonIgnore]
        public string HallNumber { get; set; }
        public sessionDto() { }

    }
}
