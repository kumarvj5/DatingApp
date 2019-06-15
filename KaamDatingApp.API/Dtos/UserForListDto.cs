using System;

namespace KaamDatingApp.API.Dtos
{
    public class UserForListDto
    {
        public int Id {get; set; }
        public string Username {get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public String City { get; set; }
        public String Country { get; set; }
        public String PhotoUrl { get; set; }
    }
}