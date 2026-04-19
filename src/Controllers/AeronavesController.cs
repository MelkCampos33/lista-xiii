using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AmericanAirlinesApi.Data;
using AmericanAirlinesApi.Models;

namespace AmericanAirlinesApi.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class AeronavesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AeronavesController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Aeronave>>> GetAll()
        {
            return await _context.Aeronaves.ToListAsync();
        }

      
        [HttpGet("{id}")]
        public async Task<ActionResult<Aeronave>> GetById(int id)
        {
            var aeronave = await _context.Aeronaves.FindAsync(id);

            if (aeronave == null)
                return NotFound($"Aeronave com Id {id} não encontrada.");

            return aeronave;
        }

    
        [HttpPost]
        public async Task<ActionResult<Aeronave>> Create(Aeronave aeronave)
        {
            _context.Aeronaves.Add(aeronave);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = aeronave.Id }, aeronave);
        }

      
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Aeronave aeronave)
        {
            if (id != aeronave.Id)
                return BadRequest("O Id da URL não confere com o Id do corpo da requisição.");

            _context.Entry(aeronave).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Verifica se o registro ainda existe (pode ter sido deletado)
                if (!_context.Aeronaves.Any(a => a.Id == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var aeronave = await _context.Aeronaves.FindAsync(id);

            if (aeronave == null)
                return NotFound();

            _context.Aeronaves.Remove(aeronave);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
