using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using U5ProgWebPartituras.Models.ViewModels;
using U5ProgWebPartituras.Services;

namespace U5ProgWebPartituras.Controllers
{
    public class HomeController : Controller
    {
        public PartituraService PartituraService { get; }
        public HomeController(PartituraService partituraService)
        {
            PartituraService = partituraService;
        }

        public IActionResult Index()
        {
            var vm = PartituraService.GetPartiturasRecientesYXGenero();
            if (vm == null)
            {
                vm = new IndexViewModel();  // Fallback
            }
            return View(vm);
            //var vm = PartituraService.GetPartiturasRecientesYXGenero();
            //return View(vm);
        }
        public IActionResult Genero(string? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var vm = PartituraService.GetPartiturasOrdenadasXGenero(id!);
            return View(vm);
        }
        public IActionResult Detalles(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("Index");
            }
            var vm = PartituraService.GetDetallesPartitura(id);
            return View(vm);
        }
    }
}
