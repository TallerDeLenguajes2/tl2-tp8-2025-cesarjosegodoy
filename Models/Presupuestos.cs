using System.Text.Json.Serialization;
using SistemaVentas.Web.Models;

namespace SistemaVentas.Web.Models
{


    public class Presupuesto
    {
        private const decimal IVA = 0.21m; // 21% de IVA
        public int IdPresupuesto { get; set; }

        public string? NombreDestinatario { get; set; }

        public DateTime FechaCreacion { get; set; } // antes DateOnly

        public List<PresupuestoDetalle> Detalle { get; set; } = new List<PresupuestoDetalle>();

      public decimal MontoPresupuesto()
        {
            // Se calcula sumando el subtotal de cada detalle (Precio * Cantidad)
            return Detalle.Sum(d => d.Producto.Precio * d.Cantidad);
        }

        public decimal MontoPresupuestoConIva()
        {
            decimal montoBase = MontoPresupuesto();
            // Retorna el monto base más el 21% del IVA
            return montoBase * (1 + IVA);
        }

        public int CantidadProductos()
        {
            return Detalle.Sum(d => d.Cantidad);
        }

        /*public Presupuesto(string nombreDestinatario)
        {
            NombreDestinatario = nombreDestinatario;
            FechaCreacion = DateOnly.FromDateTime(DateTime.Now);
        }*/




    }

}


