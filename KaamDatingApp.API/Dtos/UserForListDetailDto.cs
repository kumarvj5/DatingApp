using System;
using System.Collections.Generic;
using KaamDatingApp.API.Models;

namespace KaamDatingApp.API.Dtos
{
    public class UserForListDetailDto
    {
        public int Id {get; set; }
        public string Username {get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public String Introduction { get; set; }
        public String LookingFor { get; set; }
        public String Interests { get; set; }
        public String City { get; set; }
        public String Country { get; set; }
        public String PhotoUrl { get; set; }

        public ICollection<PhotosForDetailedDto> Photos { get; set; }
    }
}