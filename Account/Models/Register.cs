using System.ComponentModel.DataAnnotations;

namespace Registerservice.Models
{
    public class Register
    {

        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
