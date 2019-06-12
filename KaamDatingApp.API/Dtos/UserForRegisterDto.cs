using System.ComponentModel.DataAnnotations;

namespace KaamDatingApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(8,MinimumLength=5, ErrorMessage="You should enter between 5 and 8")]
        public string Password { get; set; }
    }
}