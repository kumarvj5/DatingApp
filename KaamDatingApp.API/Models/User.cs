using System;
using System.Collections.Generic;

namespace KaamDatingApp.API.Models
{
    public class User
    {
        public int Id {get; set; }
        public string Username {get; set; }
        public byte[] PasswordHash {get; set; }
        public byte[] PasswordSalt {get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public String Introduction { get; set; }
        public String LookingFor { get; set; }
        public String Interests { get; set; }
        public String City { get; set; }
        public String Country { get; set; }
        
        public ICollection<Photo> Photos {get;set;}

        public ICollection<Like> Likers { get; set; }
        public ICollection<Like> Likees { get; set; }
        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessagesReceived { get; set; }
    }
}