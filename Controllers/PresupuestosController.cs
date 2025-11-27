using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;// Necesario para SelectList
using SistemaVentas.Web.Repository;
using SistemaVentas.Web.ViewModels;
using SistemaVentas.Web.Models;


namespace SistemaVentas.Web.Controllers
{



    public class PresupuestosController : Controller
    {

        private readonly PresupuestoRepository _presupuestoRepository;// Necesitamos el repositorio de Productos para llenar el dropdown
        private readonly ProductoRepository _productoRepository;

        public PresupuestosController(PresupuestoRepository presupuestoRepository, ProductoRepository productoRepository)
        {
            _presupuestoRepository = presupuestoRepository;
            _productoRepository = productoRepository;
        }

        // - - - - - - - - - - - - - - - - - Listado

        [HttpGet]
        public IActionResult Index()
        {
            var presupuestos = _presupuestoRepository.GetAllPresupuesto();
            return View(presupuestos);
        }

        // - - - - - - - - - - - - - - - - - Detalle

        [HttpGet]
        public IActionResult Details(int id)
        {
            var presupuesto = _presupuestoRepository.GetByIdPresupuesto(id);
            if (presupuesto == null)
            {
                return NotFound();
            }

            var presupuestoVm = new PresupuestoViewModel
            {
                IdPresupuesto = presupuesto.IdPresupuesto,
                NombreDestinatario = presupuesto.NombreDestinatario,
                FechaCreacion = presupuesto.FechaCreacion
            };


            return View(presupuestoVm);
        }

        // - - - - - - - - - - - - - - - - - Crear

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//esto nuevo
        public IActionResult Create(PresupuestoViewModel _presupuestoVm) //cambio ahora es PresupuestoViewModel
        {
            if (_presupuestoVm.FechaCreacion > DateTime.Now)
            {
                ModelState.AddModelError("FechaCreacion", "La fecha no puede ser posterior a la actual.");
            }

            if (!ModelState.IsValid)
            {
                return View(_presupuestoVm);
            }

            Presupuesto p = new Presupuesto
            {
                NombreDestinatario = _presupuestoVm.NombreDestinatario,
                FechaCreacion = _presupuestoVm.FechaCreacion
            };

            _presupuestoRepository.Crear(p);

            return RedirectToAction(nameof(Index));
        }

        // - - - - - - - - - - - - - - - - - Editar

        [HttpGet]
        public IActionResult Edit(int id)
        {
            //var presupuesto = _presupuestoRepository.ObtenerPorId(id); Antes

            Presupuesto? presupuesto = _presupuestoRepository.GetByIdPresupuesto(id); // lo nuevo

            if (presupuesto == null)
                return NotFound();

            PresupuestoViewModel presupuestoVm = new PresupuestoViewModel
            {
                IdPresupuesto = presupuesto.IdPresupuesto,
                NombreDestinatario = presupuesto.NombreDestinatario,
                FechaCreacion = presupuesto.FechaCreacion
            };

            return View(presupuestoVm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PresupuestoViewModel presupuestoVm)
        {
            if (id != presupuestoVm.IdPresupuesto)
                return NotFound();

            if (!ModelState.IsValid)
            {
                return View(presupuestoVm);
            }

            Presupuesto presAEdit = new Presupuesto
            {
                IdPresupuesto = presupuestoVm.IdPresupuesto,
                NombreDestinatario = presupuestoVm.NombreDestinatario,
                FechaCreacion = presupuestoVm.FechaCreacion
            };

            _presupuestoRepository.Modificar(presAEdit);

            return RedirectToAction(nameof(Index));

        }

        // - - - - - - - - - - - - - - - - - Eliminar

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Presupuesto? presupuesto = _presupuestoRepository.GetByIdPresupuesto(id);
            if (presupuesto == null)
                return NotFound();
            var presupuestoVm = new PresupuestoViewModel
            {
                IdPresupuesto = presupuesto.IdPresupuesto,
                NombreDestinatario = presupuesto.NombreDestinatario,
                FechaCreacion = presupuesto.FechaCreacion
            };
    

            return View(presupuestoVm);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarPresupuesto(int id)
        {
            _presupuestoRepository.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }


        // - - - - - - - - - - - - - - - - - Agregar Producto al Presupuesto
        [HttpGet]
        public IActionResult AgregarProducto(int id)
        {
            List<Producto> productos = _productoRepository.GetAll();

            AgregarProductoViewModel model = new AgregarProductoViewModel
            {
                IdPresupuesto = id,
                ListaProductos = new SelectList(productos, "IdProducto", "Descripcion")

            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgregarProducto(AgregarProductoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // RE-CARGAR DROPDOWN EN UN POST INVALIDO
                var productos = _productoRepository.GetAll();
                model.ListaProductos = new SelectList(productos, "IdProducto", "Descripcion");

                return View(model);
            }

            // Guardar en la BD
            _presupuestoRepository.AgregarProducto(
                model.IdPresupuesto,
                model.IdProducto,
                model.Cantidad
            );

            return RedirectToAction(nameof(Details), new { id = model.IdPresupuesto });
        }



        /*public IActionResult AgregarProducto(AgregarProductoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // LÓGICA CRÍTICA DE RECARGA: Si falla la validación,
                // debemos recargar el SelectList porque se pierde en el POST.

                Console.WriteLine("ModelState inválido");
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"{error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                List<Producto> productos = _productoRepository.GetAll();
                model.ListaProductos = new SelectList(productos, "IdProducto", "Descripcion");
                return View(model);
            }

            // 2. Si es VÁLIDO: Llamamos al repositorio para guardar la relación
            _presupuestoRepository.AgregarProducto(model.IdPresupuesto, model.IdProducto, model.Cantidad);
            return RedirectToAction(nameof(Details), new { id = model.IdPresupuesto }); //Presupuesto/Details/5
        }*/



    }
}