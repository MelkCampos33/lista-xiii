using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AmericanAirlinesApi.Data;
using AmericanAirlinesApi.Models;

namespace AmericanAirlinesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VoosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Voo>>> GetAll()
        {
            return await _context.Voos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Voo>> GetById(int id)
        {
            var voo = await _context.Voos.FindAsync(id);

            if (voo == null)
                return NotFound($"Voo com Id {id} não encontrado.");

            return voo;
        }

       
        [HttpPost]
        public async Task<ActionResult<Voo>> Create(Voo voo)
        {
            var aeronaveExiste = await _context.Aeronaves.AnyAsync(a => a.Id == voo.AeronaveId);
            if (!aeronaveExiste)
                return NotFound($"Aeronave com Id {voo.AeronaveId} não encontrada.");

            var aeronaveEmVoo = await _context.Voos.AnyAsync(v =>
                v.AeronaveId == voo.AeronaveId && v.Status == "Em Voo");

            if (aeronaveEmVoo)
                return Conflict("Aeronave indisponível, encontra-se em trânsito.");

            voo.Status = "Agendado";

            _context.Voos.Add(voo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = voo.Id }, voo);
        }

     
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string novoStatus)
        {
            var voo = await _context.Voos.FindAsync(id);

            if (voo == null)
                return NotFound($"Voo com Id {id} não encontrado.");

            var statusValidos = new[] { "Agendado", "Em Voo", "Finalizado", "Cancelado" };
            if (!statusValidos.Contains(novoStatus))
                return BadRequest($"Status inválido. Use: {string.Join(", ", statusValidos)}");

            if ((voo.Status == "Finalizado" || voo.Status == "Cancelado") && novoStatus == "Em Voo")
                return UnprocessableEntity(
                    $"Operação inválida: voo com status '{voo.Status}' não pode voltar para 'Em Voo'.");

            voo.Status = novoStatus;
            await _context.SaveChangesAsync();

            return Ok(voo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var voo = await _context.Voos.FindAsync(id);

            if (voo == null)
                return NotFound();

            _context.Voos.Remove(voo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
