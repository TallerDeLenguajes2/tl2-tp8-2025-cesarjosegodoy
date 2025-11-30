using System.Collections.Generic;
using SistemaVentas.Web.Models; // Asegúrate de tener los using correctos

namespace MVC.Interfaces
{
    // CUMPLE DI: Abstracción para el Repositorio de Presupuestos
    public interface IPresupuestoRepository
    {
        List<Presupuesto> GetAllPresupuesto();

        int Crear(Presupuesto p);
        
        Presupuesto GetByIdPresupuesto(int id);
        
        void Modificar(Presupuesto presupuesto);
        
        bool Eliminar(int id);
        
        // Método clave del TP para la relación N:M
        void AgregarProducto(int idPresupuesto, int idProducto, int cantidad);

        List<PresupuestoDetalle> GetDetallesByPresupuestoId(int idPresupuesto);

        void AddDetalle(int idPresupuesto, int idProducto, int cantidad);
    }
}