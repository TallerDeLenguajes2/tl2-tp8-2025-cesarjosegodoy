using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RepositoriesP;
using Presupuestos;

public class PresupuestosController : Controller
{
    private PresupuestoRepository _presupuestoRepository;

    public PresupuestosController()
    {
        _presupuestoRepository = new PresupuestoRepository();
    }

    [HttpGet]
    public IActionResult Index()
    {
        var presupuestos = _presupuestoRepository.Listar();
        return View(presupuestos);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var presupuesto = _presupuestoRepository.ObtenerPorId(id);
        return View(presupuesto);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Presupuesto _presupuesto)
    {
        _presupuestoRepository.Crear(_presupuesto);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var presupuesto = _presupuestoRepository.ObtenerPorId(id);
        return View(presupuesto);
    }

    [HttpPost]
    public IActionResult Edit(Presupuesto presupuesto)
    {
        _presupuestoRepository.Modificar(presupuesto);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var presupuesto = _presupuestoRepository.ObtenerPorId(id);
        return View(presupuesto);
    }
    [HttpPost, ActionName("Delete")]
    public IActionResult EliminarPresupuesto(int id)
    {
        _presupuestoRepository.Eliminar(id);
        return RedirectToAction(nameof(Index));
    }


}
