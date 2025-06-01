using CinemaAppWPF.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaAppWPF.Session
{
    public class UserSession
    {
        private static UserSession _instance;

        public static UserSession Instance => _instance ??= new UserSession();

        public UserDto LoggedInUser { get; set; }

        private UserSession() { } 
    }
}
