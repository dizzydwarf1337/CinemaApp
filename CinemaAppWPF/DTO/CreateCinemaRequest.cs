using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaAppWPF.DTO
{
    public class CreateCinemaRequest
    {
        public CinemaDto CinemaDto { get; set; }
        public int NumOfHalls { get; set; }
    }
}
