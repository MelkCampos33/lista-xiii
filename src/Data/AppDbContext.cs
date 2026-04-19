using Microsoft.EntityFrameworkCore;
using AmericanAirlinesApi.Models;

namespace AmericanAirlinesApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Aeronave>   Aeronaves   { get; set; }
        public DbSet<Tripulante> Tripulantes { get; set; }
        public DbSet<Voo>        Voos        { get; set; }
        public DbSet<Reserva>    Reservas    { get; set; }
    }
}
