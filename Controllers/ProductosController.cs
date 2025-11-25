using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tp8.Models;
using ProductoRepotitorys;
using Productos;

public class ProductosController : Controller
{

    private readonly ProductoRepository _productoRepository;

    public ProductosController()
    {
        _productoRepository = new ProductoRepository();
    }


    /*private readonly ILogger<ProductosController> _logger;

    public ProductosController(ILogger<ProductosController> logger)
    {
        _logger = logger;
    }*/
    [HttpGet]
    public IActionResult Index() // views
    {                                                  //funcion en repositories
        List<Producto> productos = _productoRepository.GetAll(); // mostrar todo
        return View(productos);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var producto = _productoRepository.GetById(id); //aqui

        if (producto == null)
            return NotFound($"No se encontró el presupuesto con ID {id}");
        return View(producto);


    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Producto producto)
    {
        _productoRepository.Alta(producto);
        return RedirectToAction(nameof(Index));
    }


    [HttpGet]
    public IActionResult Edit(int id)
    {
        var producto = _productoRepository.GetById(id);
        return View(producto);
    }
    [HttpPost]
    public IActionResult Edit(Producto producto)
    {
        _productoRepository.Modificar(producto.IdProducto, producto.Descripcion, producto.Precio); // ver modificar
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var producto = _productoRepository.GetById(id);
        return View(producto);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult EliminarProducto(int id)// y esto ver
    {
        _productoRepository.Eliminar(id);
        return RedirectToAction(nameof(Index));
    }




}

