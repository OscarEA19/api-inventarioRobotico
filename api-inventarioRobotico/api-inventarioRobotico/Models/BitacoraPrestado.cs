namespace api_inventarioRobotico.Models
{
    public class BitacoraPrestado
    {
        public int id { get; set; }
        public string? cedulaUsuario { get; set; }
        public string? idKit { get; set; }
        public DateTime fecha { get; set; }

    }
}
