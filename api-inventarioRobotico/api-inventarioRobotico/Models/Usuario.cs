namespace api_inventarioRobotico.Models
{
    public class Usuario
    {
        public string? cedula { get; set; }
        public string? carnet { get; set; }
        public string? nombre { get; set; }
        public string? apellido { get; set; }
        public int idRol { get; set; }
        public string? contrasena { get; set; }
        public string? correoElectronico { get; set; }
    }
}
