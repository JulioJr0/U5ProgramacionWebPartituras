using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index(IndexViewModel vm)
        {
            return View();
        }
        public IActionResult Genero(GeneroViewModel vm)
        {
            return View();
        }
        public IActionResult Detalles(DetallesViewModel vm)
        {
            return View();
        }
    }
}
