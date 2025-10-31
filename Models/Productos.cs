using System.Text.Json.Serialization;

namespace Productos
{


    public class Producto
    {
       [JsonPropertyName("id")]
        public int IdProducto { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }

        [JsonPropertyName("precio")]
        public int Precio { get; set; }

        public Producto(){}

        public Producto(int _id, string _descripcion, int _precio)
        {
            IdProducto = _id;
            Descripcion = _descripcion;
            Precio = _precio;
        }
    }

}