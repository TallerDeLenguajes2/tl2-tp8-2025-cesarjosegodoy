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
    public IActionResult Index()
    {
        List<Producto> productos = _productoRepository.GetAll();
        return View(productos);
    }

}

