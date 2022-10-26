using AutoMapper;

using InventoryService.Data;
using InventoryService.Data.Inventory;
using InventoryService.Models;

using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        private AppDbContext _context;
        private IMapper _mapper;

        public InventoryController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
       
        [HttpPost]
        public IActionResult AddProduct([FromBody] CreateProductDto productDto)
        {
            Stock stock = _mapper.Map<Stock>(productDto);
            _context.Inventories.Add(stock);
            _context.SaveChanges();
            return Ok(stock);
            
            //return CreatedAtAction(nameof(SearchInventoryId), new { Id = inventory.Id }, inventory);
        }
    }
}
