namespace AmericanAirlinesApi.Models
{
 
    public class Voo
    {
        public int Id { get; set; }

        public string CodigoVoo { get; set; } = string.Empty;

        public string Origem { get; set; } = string.Empty;

        public string Destino { get; set; } = string.Empty;

        public int AeronaveId { get; set; }

        public string Status { get; set; } = "Agendado";
    }
}
