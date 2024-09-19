namespace SistemaWebMedida.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string? NombreCompleto { get; set; }
        public string? CorreoUsuario { get; set; }
        public string? Clave { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string? Direccion { get; set; }
        public string? NumeroTelefono { get; set; }
    }
}
