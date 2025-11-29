using SistemaVentas.Web.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;


namespace SistemaVentas.Web.ViewModels
{
    public class PresupuestoViewModel
    {
        public int IdPresupuesto { get; set; }
        // Validación: Requerido
        [Display(Name = "Nombre o Email del Destinatario")]
        [Required(ErrorMessage = "El nombre o email es obligatorio.")]
        // Opcional: Se puede añadir la validación de formato de email si se opta por guardar el mail.
        // [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        public string NombreDestinatario { get; set; } = string.Empty;
        // Validación: Requerido y tipo de dato
        [Display(Name = "Fecha de Creación")]
        [Required(ErrorMessage = "La fecha es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaCreacion { get; set; } = DateTime.Today; // TIP: usar Today evita mostrar 01/01/0001 al cargar el formulario
        // La validación de que la fecha no es futura se hará en el Controlador (ver Etapa 3).
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

    }
}