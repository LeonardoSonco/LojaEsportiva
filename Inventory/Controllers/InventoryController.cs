using AutoMapper;

using InventoryService.Data;
using InventoryService.Data.Inventory;
using InventoryService.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
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
        public IActionResult AddProduct([FromBody] CreateProductDto productDto) // IActionResult, tem funções especificas de retorno
        {
            Stock stock = _mapper.Map<Stock>(productDto); // Converte um productDto para um stock
            _context.Inventories.Add(stock); // adiciona o produto no banco
            _context.SaveChanges(); // salva operação no banco
            return CreatedAtAction(nameof(SearchInventoryId), new { Id = stock.Id }, stock); //Indica a ação de criação do filme
        }

        
        [HttpGet]
        //[Authorize(Roles = "manager")]
        public IActionResult SearchInventory()
        {

            return Ok(_context.Inventories); // Retorna todos os produtos (todo o conjunto de dados)
        }
        
        [HttpGet("{id}")]
        public IActionResult SearchInventoryId(int id)
        {
            Stock stock = _context.Inventories.FirstOrDefault(stock => stock.Id == id); // procura por o id passado no banco se não achar retorna null

            if (stock != null)
            {
                ReadProductDto productDto = _mapper.Map<ReadProductDto>(stock); // converte um produto para um productDto e passa os valores
                return Ok(productDto); // mostra o produto buscado
            }
            return NotFound();
        }


        [HttpPut("{id}")]
        public IActionResult AtualizaFilme(int id, [FromBody] UpdateProductDto productDto) // recebe a partir do corpo da requisição recebe as novas informações
        {
            Stock stock = _context.Inventories.FirstOrDefault(stock => stock.Id == id); // procura por o id passado no banco se não achar retorna null

            if (stock == null)
            {
                return NotFound(); // Informa que o filme não foi encontrado
            }

            _mapper.Map(productDto, stock); // converte um productDto para um produto
            _context.SaveChanges(); // salva a atualização no banco, e passa os novos valores
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult DeletaFilme(int id)
        {

            Stock stock = _context.Inventories.FirstOrDefault(stock => stock.Id == id);

            if (stock == null)
            {
                return NotFound(); // Informa que o filme não foi encontrado
            }
            _context.Remove(stock); // remove do banco o produto pelo o id passado
            _context.SaveChanges();// salva a atualização no banco
            return NoContent();
        }

    }
}
