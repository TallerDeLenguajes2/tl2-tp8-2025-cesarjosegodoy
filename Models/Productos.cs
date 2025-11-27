using System.Text.Json.Serialization;
using SistemaVentas.Web.Models;

namespace SistemaVentas.Web.Models
{


    public class Producto
    {
       [JsonPropertyName("id")]
        public int IdProducto { get; set; }

        [JsonPropertyName("descripcion")]
        public string? Descripcion { get; set; }

        [JsonPropertyName("precio")]
        public decimal Precio { get; set; }

        public Producto(){}

        public Producto(int _id, string _descripcion, int _precio)
        {
            IdProducto = _id;
            Descripcion = _descripcion;
            Precio = _precio;
        }
    }

}