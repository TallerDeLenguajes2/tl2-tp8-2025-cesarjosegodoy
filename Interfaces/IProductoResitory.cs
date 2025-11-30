using System.Collections.Generic;
using SistemaVentas.Web.Models; // Asume que Producto está en la carpeta Models

namespace MVC.Interfaces
{
    // CUMPLE DI: Abstracción del Repositorio de Productos
    public interface IProductoRepository
    {
        // El método Add recibe un Producto para dar de alta
        int Add(Producto producto);

        // El método GetAll devuelve una lista de Producto
        List<Producto> GetAll();
        
        // El método GetById devuelve un único Producto o null
        Producto? GetById(int id);
        
        // El método Update recibe un Producto para modificar
        void Modificar(Producto nuevo);
        
        // El método Delete recibe el ID del producto a eliminar
        void Eliminar(int _id);
    }
}