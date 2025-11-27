using SistemaVentas.Web.Models;

namespace SistemaVentas.Web.Models
{
    public class PresupuestoDetalle
    {
        public int IdDetalle { get; set; }
        public int IdPresupuesto { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public Producto? Producto { get; set; }
        public decimal Subtotal => Cantidad * PrecioUnitario;

    }
}