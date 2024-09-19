namespace SistemaWebMedida.Models
{
    public class ReporteDatos
    {
        public int Id { get; set; }
        public decimal PresionAtm { get; set; }
        public decimal PresionDif { get; set; }
        public decimal Co2 { get; set; }
        public decimal Temperatura { get; set; }
        public decimal Oxigeno { get; set; }
        public DateTime Fecha { get; set; }

    }
}
