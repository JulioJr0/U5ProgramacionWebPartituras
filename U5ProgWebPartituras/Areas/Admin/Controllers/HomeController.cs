using Microsoft.AspNetCore.Mvc;
using U5ProgWebPartituras.Areas.Admin.Models;
using U5ProgWebPartituras.Services;

namespace U5ProgWebPartituras.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public GenerosService GenerosService { get; }
        public PartituraService PartituraService { get; }

        public HomeController(GenerosService generosService, PartituraService partituraService)
        {
            GenerosService = generosService;
            PartituraService = partituraService;
        }

        public IActionResult Index()
        {
            var vm = GenerosService.ListaGeneros();
            return View(vm);
        }

        [HttpGet]
        public IActionResult Agregar()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult Agregar(AgregarAdminPartiturasViewMode vm)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public IActionResult Editar()
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public IActionResult Editar(EditarAdminPartiturasViewModel vm)
        {
            throw new NotImplementedException();
        }
        [HttpGet]
        public IActionResult Eliminar()
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public IActionResult Eliminar(EliminarAdminPartiturasViewModel vm)
        {
            throw new NotImplementedException();
        }

    }
}
