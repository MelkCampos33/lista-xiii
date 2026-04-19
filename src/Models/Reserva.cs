namespace AmericanAirlinesApi.Models
{

    public class Reserva
    {
        public int Id { get; set; }

       
        public int VooId { get; set; }

        public string NomePassageiro { get; set; } = string.Empty;

       
        public string Assento { get; set; } = string.Empty;

                public decimal Valor { get; set; } = 0;
    }
}
