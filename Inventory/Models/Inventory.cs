using System.ComponentModel.DataAnnotations;

namespace InventoryService.Models
{
    public class Stock
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double Price { get; set; }

        public string Description { get; set; }
    }
}
