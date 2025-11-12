using Microsoft.AspNetCore.Mvc;
using U5ProgWebPartituras.Services;

namespace U5ProgWebPartituras.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        public GenerosService GenerosService { get; }
        public HomeController(GenerosService generosService)
        {
            GenerosService = generosService;
        }

        public IActionResult Index()
        {
            var vm = GenerosService.ListaGeneros();
            return View(vm);
        }
    }
}
