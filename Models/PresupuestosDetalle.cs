using System.Text.Json.Serialization;
using Productos;

namespace PresupuestoDetalles
{
    public class PresupuestoDetalle
    {
        [JsonPropertyName("producto")]
        public Producto? Producto { get; set; }

        [JsonPropertyName("cantidad")]
        public int Cantidad { get; set; }

        /*
        public PresupuestoDetalle(Producto _producto, int _cantidad)
        {
            Producto = _producto;
            Cantidad = _cantidad;
        }*/
    }
}