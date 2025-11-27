using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaVentas.Web.Repository;
using SistemaVentas.Web.Models;
using SistemaVentas.Web.ViewModels;

namespace SistemaVentas.Web.Controllers
{
    public class ProductosController : Controller
    {

        private readonly ProductoRepository _productoRepository;

        public ProductosController()
        {
            _productoRepository = new ProductoRepository();
        }

        // - - - - - - - - - - - - - - - - - Listado

        [HttpGet]
        public IActionResult Index() // views
        {                                                  //funcion en repositories
            List<Producto> productos = _productoRepository.GetAll(); // mostrar todo
            return View(productos);
        }

        // - - - - - - - - - - - - - - - - - Detalle Listado x id
        [HttpGet]
        public IActionResult Details(int id)
        {
            var producto = _productoRepository.GetById(id); //aqui

            if (producto == null)
                return NotFound($"No se encontró el presupuesto con ID {id}");

            var productoVm = new ProductoViewModel // con esto
            {
                IdProducto = producto.IdProducto,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio
            };


            return View(productoVm); //envía un ViewModel


        }

        // - - - - - - - - - - - - - - - - - Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductoViewModel productoVm)
        {
            // 1. CHEQUEO DE SEGURIDAD DEL SERVIDOR
            if (!ModelState.IsValid)
            {
                // Si falla: Devolvemos el ViewModel con los datos y errores a la Vista
                return View(productoVm);
            }
            // 2. SI ES VÁLIDO: Mapeo Manual de VM a Modelo de Dominio
            Producto nuevoProducto = new Producto
            {
                Descripcion = productoVm.Descripcion,
                Precio = productoVm.Precio
            };
            // 3. Llamada al Repositorio
            _productoRepository.Alta(nuevoProducto);
            return RedirectToAction(nameof(Index));


        }

        // - - - - - - - - - - - - - - - - - Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var prod = _productoRepository.GetById(id);
            if (prod == null)
                return NotFound();

            var productoVm = new ProductoViewModel
            {
                IdProducto = prod.IdProducto,
                Descripcion = prod.Descripcion,
                Precio = prod.Precio
            };

            return View(productoVm);
        }

        [HttpPost]
        public IActionResult Edit(int id, ProductoViewModel productoVm)
        {
            if (id != productoVm.IdProducto)
                return NotFound();

            // 1. CHEQUEO DE SEGURIDAD DEL SERVIDOR
            if (!ModelState.IsValid)
            {
                return View(productoVm);
            }

            // 2. Mapeo Manual de VM a Modelo de Dominio
            Producto productoAEditar = new Producto
            {
                IdProducto = productoVm.IdProducto,// Necesario para el UPDATE
                Descripcion = productoVm.Descripcion,
                Precio = productoVm.Precio
            };

            // 3. Llamada al Repositorio
            _productoRepository.Modificar(productoAEditar);
            return RedirectToAction(nameof(Index));

        }

        // - - - - - - - - - - - - - - - - - Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var producto = _productoRepository.GetById(id);

            if (producto == null)
                return NotFound();
                
            var productoVm = new ProductoViewModel // con esto
            {
                IdProducto = producto.IdProducto,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio
            };

            return View(productoVm);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult EliminarProducto(int id)// y esto ver
        {
            _productoRepository.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }




    }

}
