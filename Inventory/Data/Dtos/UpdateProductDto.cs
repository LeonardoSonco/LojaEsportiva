using System.ComponentModel.DataAnnotations;

namespace InventoryService.Data.Inventory
{
    public class UpdateProductDto
    {

        [Required]
        public string Name { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double Price { get; set; }

        public string Description { get; set; }

    }
}
