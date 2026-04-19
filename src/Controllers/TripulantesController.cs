using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using AmericanAirlinesApi.Data;
using AmericanAirlinesApi.Models;

namespace AmericanAirlinesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripulantesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TripulantesController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/tripulantes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tripulante>>> GetAll()
        {
            return await _context.Tripulantes.ToListAsync();
        }

        // GET api/tripulantes/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Tripulante>> GetById(int id)
        {
            var tripulante = await _context.Tripulantes.FindAsync(id);

            if (tripulante == null)
                return NotFound($"Tripulante com Id {id} não encontrado.");

            return tripulante;
        }

        [HttpPost]
        public async Task<ActionResult<Tripulante>> Create(Tripulante tripulante)
        {
            var funcoesValidas = new[] { "Piloto", "Copiloto", "Comissário" };
            if (!funcoesValidas.Contains(tripulante.Funcao))
                return BadRequest($"Função inválida. Use: {string.Join(", ", funcoesValidas)}");

            _context.Tripulantes.Add(tripulante);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = tripulante.Id }, tripulante);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tripulante = await _context.Tripulantes.FindAsync(id);

            if (tripulante == null)
                return NotFound();

            _context.Tripulantes.Remove(tripulante);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
