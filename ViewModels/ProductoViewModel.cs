using System.ComponentModel.DataAnnotations;
using SistemaVentas.Web.Models;
//Este VM manejará la creación y edición de productos


namespace SistemaVentas.Web.ViewModels
{
    public class ProductoViewModel
    {

        public int IdProducto { get; set; } // Se incluye Id para la acción de EDICIÓN

        [Display(Name = "Descripción del Producto")] // Validación: Máximo 250 caracteres. Es opcional por defecto si no tiene [Required]
        [StringLength(250, ErrorMessage = "La descripción no puede superar los 250 caracteres.")]
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string? Descripcion { get; set; } = string.Empty;

        [Display(Name = "Precio Unitario")] // Validación: Requerido y debe ser positivo
        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser un valor positivo.")]

        public decimal Precio { get; set; }

        public ProductoViewModel()
        {

        }

        public ProductoViewModel(Producto producto)
        {
            Descripcion = producto.Descripcion;
            IdProducto = producto.IdProducto;
            Precio = producto.Precio;
        }


    }
}