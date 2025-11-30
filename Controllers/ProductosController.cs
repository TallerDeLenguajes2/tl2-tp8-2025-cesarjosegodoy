using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaVentas.Web.Repository;
using SistemaVentas.Web.Models;
using SistemaVentas.Web.ViewModels;
using MVC.Interfaces;





namespace SistemaVentas.Web.Controllers
{
    public class ProductosController : Controller
    {

        private IProductoRepository _productoRepository;

        private IAuthenticationService _authService;

        public ProductosController(IProductoRepository prodRepo, IAuthenticationService authService)
        {
            _productoRepository = prodRepo;

            _authService = authService;
        }


        // - - - - - - - - - - - - - - - - - Listado

        [HttpGet]
        public IActionResult Index() // views
        {
            // Aplicamos el chequeo de seguridad
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

            //funcion en repositories
            List<Producto> productos = _productoRepository.GetAll(); // mostrar todo
            return View(productos);
        }

        // - - - - - - - - - - - - - - - - - Detalle Listado x id
        [HttpGet]
        public IActionResult Details(int id)
        {

            // Aplicamos el chequeo de seguridad
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;


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
            // Aplicamos el chequeo de seguridad
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

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
            _productoRepository.Add(nuevoProducto);
            return RedirectToAction(nameof(Index));


        }

        // - - - - - - - - - - - - - - - - - Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {

            // Aplicamos el chequeo de seguridad
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;



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

            // Aplicamos el chequeo de seguridad
            var securityCheck = CheckAdminPermissions();
            if (securityCheck != null) return securityCheck;

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        private IActionResult CheckAdminPermissions()
        {
            // 1. No logueado? -> Login (Punto 2.e.iii)
            if (!_authService.IsAuthenticated())
            {
                return RedirectToAction("Index", "Login");
            }

            // 2. No es Administrador? -> Error (Punto 2.e.i)
            if (!_authService.HasAccessLevel("Administrador"))
            {
                // Usamos Error403 o redirigimos al Login si no existe vista de error.
                return RedirectToAction(nameof(AccesoDenegado));
            }
            return null; // Permiso concedido
        }

        public IActionResult AccesoDenegado()
        {
            // El usuario está logueado, pero no tiene el rol suficiente.
            return View();
        }


    }

}
