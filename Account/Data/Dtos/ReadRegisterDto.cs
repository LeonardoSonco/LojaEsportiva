using System.ComponentModel.DataAnnotations;

namespace Registerservice.Data.Dtos
{
    public class ReadRegisterDto
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
