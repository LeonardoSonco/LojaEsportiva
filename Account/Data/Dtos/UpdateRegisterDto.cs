using System.ComponentModel.DataAnnotations;

namespace Registerservice.Data.Dtos
{
    public class UpdateRegisterDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        
    }
}
