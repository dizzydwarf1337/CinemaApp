using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaAppWPF.DTO
{

        public class ticketDto
        {
            public Guid Id { get; set; }
            public Guid SessionId { get; set; }
            public string Seat { get; set; }
            public string Status { get; set; }
            public double Price { get; set; }
            public virtual DateTime Created { get; set; } = DateTime.Now;
            public int NumberOfSeats { get; set; }
            public Guid UserId { get; set; }
            public ticketDto() { }
        }
}
