using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tp8.Models;

public class PresupuestosController : Controller
{

    private readonly ILogger<PresupuestosController> _logger;

    public PresupuestosController(ILogger<PresupuestosController> logger)
    {
        _logger = logger;
    }
    public IActionResult Index()
    {
        return View();
    }

}
