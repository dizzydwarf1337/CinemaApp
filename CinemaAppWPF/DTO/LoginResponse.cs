using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaAppWPF.DTO
{
    public class LoginResponse
    {
        public string Message { get; set; }
        public UserDto User { get; set; }
    }
}
