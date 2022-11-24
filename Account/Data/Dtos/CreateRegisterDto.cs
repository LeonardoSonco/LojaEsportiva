using System.ComponentModel.DataAnnotations;

namespace Registerservice.Data.Dtos
{
    public class CreateRegisterDto
    { 
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }   
        
    }
}
