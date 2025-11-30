using SistemaVentas.Web.Models;

namespace SistemaVentas.Web.Models
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string? Descripcion { get; set; }

        public decimal Precio { get; set; }

        public Producto() { }
        public Producto(int _id, string _descripcion, int _precio)
        {
            IdProducto = _id;
            Descripcion = _descripcion;
            Precio = _precio;
        }
    }

}