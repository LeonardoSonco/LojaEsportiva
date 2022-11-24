using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Model
{
    public class UserLogin
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
