using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaAppWPF.DTO
{
    public class hallDto
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public int Seats { get; set; }
        public Guid CinemaId { get; set; }
        public hallDto() { }
    }
}
