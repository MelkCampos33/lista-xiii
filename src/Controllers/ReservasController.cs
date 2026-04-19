using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AmericanAirlinesApi.Data;
using AmericanAirlinesApi.Models;

namespace AmericanAirlinesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReservasController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/reservas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetAll()
        {
            return await _context.Reservas.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reserva>> GetById(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva == null)
                return NotFound($"Reserva com Id {id} não encontrada.");

            return reserva;
        }

        [HttpGet("voo/{vooId}")]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetByVoo(int vooId)
        {
            var reservas = await _context.Reservas
                .Where(r => r.VooId == vooId)
                .ToListAsync();

            return Ok(reservas);
        }

        
        [HttpPost]
        public async Task<ActionResult<Reserva>> Create(Reserva reserva)
        {
            var voo = await _context.Voos.FindAsync(reserva.VooId);
            if (voo == null)
                return NotFound($"Voo com Id {reserva.VooId} não encontrado.");

            var aeronave = await _context.Aeronaves.FindAsync(voo.AeronaveId);
            if (aeronave == null)
                return NotFound("Aeronave do voo não encontrada.");

            var totalReservas = await _context.Reservas
                .CountAsync(r => r.VooId == reserva.VooId);

            if (totalReservas >= aeronave.CapacidadePassageiros)
                return BadRequest("Voo lotado. Não é possível realizar novas reservas.");

            string assentoUpper = reserva.Assento.ToUpper();
            if (assentoUpper.EndsWith("A") || assentoUpper.EndsWith("F"))
            {
                reserva.Valor += 50.00m;
                Console.WriteLine(" Assento na janela reservado com sucesso!");
            }

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = reserva.Id }, reserva);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva == null)
                return NotFound();

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
