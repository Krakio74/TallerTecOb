using SQLite;

namespace ObligatorioTTec.Models
{
    public class Sucursal
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Ubicacion { get; set; }
        public string Categoria { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Icono { get; set; }
    }
}
