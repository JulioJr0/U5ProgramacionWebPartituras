using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using U5ProgWebPartituras.Areas.Admin.Models;
using U5ProgWebPartituras.Services;

namespace U5ProgWebPartituras.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public GenerosService GenerosService { get; }
        public PartituraService PartituraService { get; }
        public IWebHostEnvironment HostEnvironment { get; }

        public HomeController(GenerosService generosService, PartituraService partituraService, IWebHostEnvironment webHostEnvironment)
        {
            GenerosService = generosService;
            PartituraService = partituraService;
            HostEnvironment = webHostEnvironment;
        }
        //[HttpGet]
        //public IActionResult Index()
        //{
        //    var vm = PartituraService.GetAllPartituras(0);
        //    return View(vm);
        //}
        [HttpGet]
        public IActionResult Index(int? idFiltroGenero)
        {
            var vm = PartituraService.GetAllPartituras(idFiltroGenero);
            vm.IdGeneroSeleccionado = idFiltroGenero??0;
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
            if (vm.Pdf == null)
            {
                ModelState.AddModelError(nameof(vm.Pdf), "Debe seleccionar un archivo PDF o imagen de la partitura.");
            }
            else
            {
                var extPdf = Path.GetExtension(vm.Pdf.FileName).ToLower();
                if (extPdf != ".pdf" && extPdf != ".jpg" && extPdf != ".jpeg")
                {
                    ModelState.AddModelError(nameof(vm.Pdf), "El formato de partitura no está permitido (solo .pdf, .jpg, .jpeg).");
                }
                if (vm.Pdf.Length > 1024 * 1024 * 5) // 5MB
                {
                    ModelState.AddModelError(nameof(vm.Pdf), "La partitura es mayor a 4MB.");
                }
            }
            //validar Audio (OPCIONAL)
            if (vm.Audio != null)
            {
                var extAudio = Path.GetExtension(vm.Audio.FileName).ToLower();
                if (extAudio != ".mp3" && extAudio != ".wav")
                {
                    ModelState.AddModelError(nameof(vm.Audio), "El formato de audio no está permitido (solo .mp3, .wav).");
                }
                if (vm.Audio.Length > 1024 * 1024 * 10) // 10MB
                {
                    ModelState.AddModelError(nameof(vm.Audio), "El archivo de audio es mayor a 10MB.");
                }
            }

            if (ModelState.IsValid)
            {
                int idPartitura = PartituraService.Agregar(vm);

                if (idPartitura != 0)
                {
                    //guardar PDF/Imagen (OBLIGATORIO)
                    var extPdf = Path.GetExtension(vm.Pdf!.FileName);
                    var rutaPdf = Path.Combine(HostEnvironment.WebRootPath, "partituras", $"{idPartitura}{extPdf}");

                    Directory.CreateDirectory(Path.Combine(HostEnvironment.WebRootPath, "partituras"));

                    FileStream archivoPartitura = System.IO.File.Create(rutaPdf);
                    vm.Pdf.CopyTo(archivoPartitura);
                    archivoPartitura.Close();

                    //guardar Audio (OPCIONAL)
                    if (vm.Audio != null)
                    {
                        var extAudio = Path.GetExtension(vm.Audio.FileName);
                        var rutaAudio = Path.Combine(HostEnvironment.WebRootPath, "audios", $"{idPartitura}{extAudio}");

                        Directory.CreateDirectory(Path.Combine(HostEnvironment.WebRootPath, "audios"));

                        FileStream archivoAudio = System.IO.File.Create(rutaAudio);
                        vm.Audio.CopyTo(archivoAudio);
                        archivoAudio.Close();
                    }
                    return RedirectToAction("Index");
                }
            }
            var dropdowns = PartituraService.GetForDropdowns();
            vm.GenerosLista = dropdowns.GenerosLista;
            vm.CompositoresLista = dropdowns.CompositoresLista;
            return View(vm);

            // Si ModelState NO es válido o la BD falló:
            //if (!ModelState.IsValid)
            //{
            //    vm.CompositoresLista = PartituraService.GetForDropdowns().CompositoresLista;
            //    vm.GenerosLista = PartituraService.GetForDropdowns().GenerosLista;
            //    return View(vm);
            //}
            //PartituraService.Agregar(vm);
            //return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            //var vm = PartituraService.GetForEditar(id);
            //vm.GenerosLista = PartituraService.GetForDropdowns().GenerosLista;
            //vm.CompositoresLista = PartituraService.GetForDropdowns().CompositoresLista;
            //return View(vm);
            var vm = PartituraService.GetForEditar(id);
            if (vm == null) return RedirectToAction("Index");
            return View(vm);
        }
        [HttpPost]
        public IActionResult Editar(EditarAdminPartiturasViewModel vm)
        {
            //validar PDF si se subió uno nuevo (Opcional en edición)
            if (vm.Pdf != null)
            {
                var extPdf = Path.GetExtension(vm.Pdf.FileName).ToLower();
                if (extPdf != ".pdf" && extPdf != ".jpg" && extPdf != ".jpeg")
                {
                    ModelState.AddModelError(nameof(vm.Pdf), "Formato inválido.");
                }
            }
            //validar Audio si se subió uno nuevo
            if (vm.Audio != null)
            {
                var extAudio = Path.GetExtension(vm.Audio.FileName).ToLower();
                if (extAudio != ".mp3" && extAudio != ".wav")
                {
                    ModelState.AddModelError(nameof(vm.Audio), "Formato inválido.");
                }
            }

            if (ModelState.IsValid)
            {
                bool exito = PartituraService.Editar(vm);

                if (exito)
                {
                    //reemplazar PDF/Imagen si se subió uno nuevo ---
                    if (vm.Pdf != null)
                    {
                        var extPdf = Path.GetExtension(vm.Pdf.FileName).ToLower();
                        var rutaPdf = Path.Combine(HostEnvironment.WebRootPath, "partituras", vm.Id + extPdf);

                        //eliminar archivos viejos con el mismo ID pero diferente extensión (limpieza)
                        string[] posiblesExt = { ".pdf", ".jpg", ".jpeg" };
                        foreach (var ext in posiblesExt)
                        {
                            var rutaVieja = Path.Combine(HostEnvironment.WebRootPath, "partituras", vm.Id + ext);
                            if (System.IO.File.Exists(rutaVieja)) System.IO.File.Delete(rutaVieja);
                        }

                        FileStream archivoPartitura = System.IO.File.Create(rutaPdf);
                        vm.Pdf.CopyTo(archivoPartitura);
                        archivoPartitura.Close();
                    }

                    //reemplazar Audio si se subió uno nuevo ---
                    if (vm.Audio != null)
                    {
                        var extAudio = Path.GetExtension(vm.Audio.FileName).ToLower();
                        var rutaAudio = Path.Combine(HostEnvironment.WebRootPath, "audios", vm.Id + extAudio);

                        //limpieza de audios viejos
                        string[] posiblesExt = { ".mp3", ".wav" };
                        foreach (var ext in posiblesExt)
                        {
                            var rutaVieja = Path.Combine(HostEnvironment.WebRootPath, "audios", vm.Id + ext);
                            if (System.IO.File.Exists(rutaVieja)) System.IO.File.Delete(rutaVieja);
                        }

                        FileStream archivoAudio = System.IO.File.Create(rutaAudio);
                        vm.Audio.CopyTo(archivoAudio);
                        archivoAudio.Close();
                    }

                    return RedirectToAction("Index");
                }
            }

            // Recargar listas si falló
            var dropdowns = PartituraService.GetForDropdowns();
            vm.GenerosLista = dropdowns.GenerosLista;
            vm.CompositoresLista = dropdowns.CompositoresLista;
            return View(vm);
            //if (!ModelState.IsValid)
            //{
            //    vm.CompositoresLista = PartituraService.GetForEditar(vm.Id).CompositoresLista;
            //    vm.GenerosLista = PartituraService.GetForDropdowns().GenerosLista;
            //    return View(vm);
            //}
            //PartituraService.Editar(vm);
            //return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var vm = PartituraService.GetForEliminar(id);
            if (vm == null) return RedirectToAction("Index");
            return View(vm);
        }
        [HttpPost]
        public IActionResult Eliminar(EliminarAdminPartiturasViewModel vm)
        {
            //eliminar de la Base de Datos
            bool exito = PartituraService.Eliminar(vm.Id);

            if (exito)
            {
                //eliminar Archivos Físicos (Controlador se encarga)
                string root = HostEnvironment.WebRootPath;

                //eliminar Partitura (buscar todas las extensiones posibles)
                string[] extPartituras = { ".pdf", ".jpg", ".jpeg" };
                foreach (var ext in extPartituras)
                {
                    var ruta = Path.Combine(root, "partituras", vm.Id + ext);
                    if (System.IO.File.Exists(ruta))
                    {
                        System.IO.File.Delete(ruta);
                    }
                }
                //eliminar Audio
                string[] extAudios = { ".mp3", ".wav" };
                foreach (var ext in extAudios)
                {
                    var ruta = Path.Combine(root, "audios", vm.Id + ext);
                    if (System.IO.File.Exists(ruta))
                    {
                        System.IO.File.Delete(ruta);
                    }
                }
                return RedirectToAction("Index");
            }

            return View(vm);
        }

    }
}
