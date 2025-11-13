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
            var vm = PartituraService.GetForDropdowns();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Agregar(AgregarAdminPartiturasViewMode vm)
        {
            if (!ModelState.IsValid)
            {
                vm.CompositoresLista = PartituraService.GetForDropdowns().CompositoresLista;
                vm.GenerosLista = PartituraService.GetForDropdowns().GenerosLista;
                return View(vm);
            }
            PartituraService.Agregar(vm);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var vm = PartituraService.GetForEditar(id);
            vm.GenerosLista = PartituraService.GetForDropdowns().GenerosLista;
            vm.CompositoresLista = PartituraService.GetForDropdowns().CompositoresLista;
            return View(vm);
        }
        [HttpPost]
        public IActionResult Editar(EditarAdminPartiturasViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.CompositoresLista = PartituraService.GetForEditar(vm.Id).CompositoresLista;
                vm.GenerosLista = PartituraService.GetForDropdowns().GenerosLista;
                return View(vm);
            }
            PartituraService.Editar(vm);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var vm = PartituraService.GetForEliminar(id);
            return View(vm);
        }
        [HttpPost]
        public IActionResult Eliminar(EliminarAdminPartiturasViewModel vm)
        {
            PartituraService.Eliminar(vm.Id);
            return RedirectToAction("Index");
        }

    }
}
