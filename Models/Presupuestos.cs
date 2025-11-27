using System.Text.Json.Serialization;
using SistemaVentas.Web.Models;

namespace SistemaVentas.Web.Models
{


    public class Presupuesto
    {
        [JsonPropertyName("idPresupuesto")]
        public int IdPresupuesto { get; set; }

        [JsonPropertyName("nombreDestinatario")]
        public string? NombreDestinatario { get; set; }

        [JsonPropertyName("fechaCreacion")]
        public DateTime FechaCreacion { get; set; } // antes DateOnly

        [JsonPropertyName("detalle")]
        public List<PresupuestoDetalle> Detalle { get; set; } = new();


        public decimal MontoPresupuesto()
        {
            return Detalle
                .Where(d => d.Producto != null)
                .Sum(d => d.Producto!.Precio * d.Cantidad);
        }

        public decimal MontoPresupuestoConIva()
        {
            return MontoPresupuesto() * 1.21m; // m indica que es un número decimal
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


