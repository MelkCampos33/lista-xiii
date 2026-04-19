using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AmericanAirlinesApi.Data;

namespace AmericanAirlinesApi.Controllers
{
    
    [ApiController]
    [Route("api/radar")]
    public class FlightRadarController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FlightRadarController(AppDbContext context)
        {
            _context = context;
        }

       
        [HttpGet("proximos-destinos")]
        public async Task<ActionResult<Dictionary<string, int>>> GetProximosDestinos()
        {
            
            await Task.Delay(2000);

            var voosAtivos = await _context.Voos
                .Where(v => v.Status == "Em Voo")
                .ToListAsync();

           
            var destinosPorQuantidade = voosAtivos
                .GroupBy(v => v.Destino)                           
                .ToDictionary(
                    grupo => grupo.Key,                          
                    grupo => grupo.Count()                        
                );

            return Ok(destinosPorQuantidade);
        }
    }
}
