using Registerservice.Data;
using Registerservice.Data.Dtos;
using Registerservice.Models;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

namespace Registerservice.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        private AppDbContext _context;
        private IMapper _mapper;

        public RegisterController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult AddProduct([FromBody] CreateRegisterDto registerDto)
        {
            Register register = _mapper.Map<Register>(registerDto);
            _context.Registers.Add(register);
            _context.SaveChanges();
            return CreatedAtAction(nameof(SearchInventoryId), new { Id = register.Id }, register);
        }

        [HttpGet]
        public IActionResult SearchAccount()
        {

            return Ok(_context.Registers);
        }

        [HttpGet("{id}")]
        public IActionResult SearchInventoryId(int id)
        {
            Register register = _context.Registers.FirstOrDefault(register => register.Id == id);


            if (register != null)
            {
                ReadRegisterDto registerDto = _mapper.Map<ReadRegisterDto>(register);
                return Ok(registerDto);
            }
            return NotFound();
        }
        /*
        [HttpPut("{id}")]
        public IActionResult AtualizaFilme(int id, [FromBody] UpdateRegisterDto registerDto) // recebe a partir do corpo da requisição recebe as novas informações
        {
            Register register = _context.Registers.FirstOrDefault(register => register.Id == id);

            if (register == null)
            {
                return NotFound(); // Informa que o filme não foi encontrado
            }

            _mapper.Map(registerDto, register); // converte um Dto para um filme

            
            filme.Titulo = filmeDto.Titulo;
            filme.Genero = filmeDto.Genero;
            filme.Duracao = filmeDto.Duracao;
            filme.Diretor = filmeDto.Diretor;
            _context.SaveChanges();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult DeletaFilme(int id)
        {

            Register register = _context.Registers.FirstOrDefault(register => register.Id == id);

            if (register == null)
            {
                return NotFound(); // Informa que o filme não foi encontrado
            }
            _context.Remove(register);
            _context.SaveChanges();
            return NoContent();
        }*/
    }
}
