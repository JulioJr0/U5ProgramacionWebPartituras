using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using U5ProgWebPartituras.Areas.Admin.Models;
using U5ProgWebPartituras.Models.Entities;
using U5ProgWebPartituras.Models.ViewModels;
using U5ProgWebPartituras.Repositories;

namespace U5ProgWebPartituras.Services
{
    public class PartituraService
    {
        public Repository<Partitura> RepositoryPartitura { get; }
        public Repository<Genero> RepositoryGenero { get; }
        public Repository<Compositor> RepositoryCompositor { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        public PartituraService(Repository<Partitura> repositoryPartitura, Repository<Genero> repositoryGenero, Repository<Compositor> repositoryCompositor,
            IWebHostEnvironment webHostEnvironment)
        {
            RepositoryPartitura = repositoryPartitura;
            RepositoryGenero = repositoryGenero;
            RepositoryCompositor = repositoryCompositor;
            WebHostEnvironment = webHostEnvironment;
        }
        //
        public int GetNumeroPartituras()
        {
            return RepositoryPartitura.GetAll().AsQueryable().Where(x => x.IdGenero == x.IdGeneroNavigation.Id).Count();
            //return RepositoryPartitura.GetAll().AsQueryable().Include(x => x.IdGenero).Where(x => x.IdGenero == x.IdGeneroNavigation.Id).Count();
        }
        public IndexViewModel GetPartiturasRecientesYXGenero()   //web push para las notificaciones
        {
            IndexViewModel vm = new();
            vm.PartiturasRecientes = RepositoryPartitura.GetAll().Take(3).AsQueryable().Include(x => x.IdCompositor).Include(x=> x.IdGenero).Select(x => new PartituraRecienteModel
                {
                    Id = x.Id,
                    Dificultad = x.Dificultad,
                    Titulo = x.Titulo,
                    Descripcion = x.Descripcion,
                    Compositor = x.IdCompositorNavigation.Nombre,
                    GeneroMusical = x.IdGeneroNavigation.Nombre
                });
            vm.GenerosMusicales = RepositoryGenero.GetAll().OrderBy(x => x.Nombre).Select(x => new GeneroMusicalModel
            {
                Id = x.Id,
                Nombre = x.Nombre,
                Descripcion = x.Descripcion,
                NumeroPartituras = GetNumeroPartituras()
            });
            return vm;
        }
        public GeneroViewModel GetPartiturasOrdenadasXGenero(string id)
        {
            var genero = RepositoryGenero.Get(id);
            if (genero!=null)
            {
                GeneroViewModel vm = new()
                {
                    Nombre = genero.Nombre,
                    Descripcion = genero.Descripcion,
                    NumeroPartituras = GetNumeroPartituras()
                };
                vm.Partituras = RepositoryPartitura.GetAll().AsQueryable().Include(x => x.IdCompositor).OrderBy(x => x.Titulo).Select(x => new PartiturasModel
                {
                    Id = x.Id,
                    Dificultad = x.Dificultad,
                    Titulo = x.Titulo,
                    Compositor = x.IdCompositorNavigation.Nombre,
                    Instrumentacion = x.Instrumentacion??"Instrumento: ?",
                    Descripcion = x.Descripcion
                });
                return vm;
            }
            else
            {
                throw new ArgumentException("Genero no encontrado");
            }

        }
        public DetallesViewModel GetDetallesPartitura(int id)
        {
            var entidad = RepositoryPartitura.GetAll().AsQueryable().Include(x => x.IdCompositorNavigation).Include(x => x.IdGeneroNavigation).Where(x=> x.Id == id).FirstOrDefault();
            if (entidad != null)
            {
                DetallesViewModel vm = new()
                {
                    Id = id,
                    Dificultad = entidad.
                    Titulo = entidad.Titulo,
                    Descripcion = entidad.Descripcion,
                    Instrumentacion = entidad.Instrumentacion??"Instrumento: ?",
                    Compositor = entidad.IdCompositorNavigation.Nombre,
                    GeneroMusical = entidad.IdGeneroNavigation.Nombre,
                    Nacionalidad = entidad.IdCompositorNavigation.Nacionalidad,
                    Biografia = entidad.IdCompositorNavigation.Biografia
                };
                return vm;
            }
            else
            {
                throw new ArgumentException("Partitura no encontrada");
            }
            //throw new NotImplementedException();
        }
        //Admin
        //Index Get
        public IndexAdminPartiturasViewModel GetAllPartituras(int? idFiltroGenero)
        {
            IndexAdminPartiturasViewModel vm = new();
            vm.Generos = RepositoryGenero.GetAll().OrderBy(x => x.Nombre).Select(x => new ItemLista
            {
                Id = x.Id,
                Nombre = x.Nombre
            });
            vm.Partituras = RepositoryPartitura.GetAll().OrderBy(x => x.Id).Select(x => new PartituraModel
            {
                Id = x.Id,
                Titulo = x.Titulo,
                Compositor = x.IdCompositorNavigation.Nombre,
                GeneroMusical = x.IdGeneroNavigation.Nombre,
                Dificultad = x.Dificultad,
                Instrumentacion = x.Instrumentacion??"Instrumento: ?"
            });
            vm.IdGeneroSeleccionado = idFiltroGenero ?? 0;
            return vm;
        }
        //Agregar
        //Get
        public AgregarAdminPartiturasViewMode GetForAgregar()
        {
            AgregarAdminPartiturasViewMode vm = new();
            vm.GenerosLista = RepositoryGenero.GetAll().OrderBy(x => x.Nombre).Select(x => new ItemLista
            {
                Id = x.Id,
                Nombre = x.Nombre
            });
            vm.CompositoresLista = RepositoryCompositor.GetAll().OrderBy(x => x.Nombre).Select(x => new ItemLista
            {
                Id = x.Id,
                Nombre = x.Nombre
            });
            return vm;
        }
        //post
        public void Agregar(AgregarAdminPartiturasViewMode vm)
        {
            var entidad = new Partitura
            {
                Id = 0,
                Titulo = vm.Titulo,
                Instrumentacion = vm.Instrumentacion,
                Dificultad = vm.Dificultad,
                Descripcion = vm.Descripcion,
                IdCompositor = vm.IdCompositor,
                IdGenero = vm.IdGenero
            };//EF cunado agrega algo, el id va como 0. 
            RepositoryPartitura.Insert(entidad); //antes de estto no sé el id autoincrementable
            var id = entidad.Id; //el valor es autoincrementable. EF lo trae.

            if (vm.Pdf != null)
            {
                AgregarPdf(vm.Pdf, id);
            }
            else
            {
                throw new ArgumentException("El archivo PDF/Partitura es obligatorio.");
            }

            if (vm.Audio != null)
            {
                AgregarAudio(vm.Audio, id);
            }
        }

        private void AgregarAudio(IFormFile audio, int idPartitura)
        {
            throw new NotImplementedException();
        }

        private void AgregarPdf(IFormFile archivo, int idPartitura)
        {
            if (archivo.Length>1024*1024*5)
            {
                throw new ArgumentException("El archivo PDF o imagen de la partitura debe ser de 5MB o menos.");
            }
            if (archivo.ContentType != "application/pdf" && archivo.ContentType != "image/jpeg")
            {
                throw new ArgumentException("Seleccione un archivo en formato PDF o JPEG.");
            }

            string extension = (archivo.ContentType == "application/pdf") ? ".pdf" : ".jpg";
            var carpetaPartituras = Path.Combine(WebHostEnvironment.WebRootPath, "partituras");
            var rutaDestino = Path.Combine(carpetaPartituras, $"{idPartitura}{extension}");
            Directory.CreateDirectory(carpetaPartituras);
            var file = File.Create(rutaDestino);
            archivo.CopyTo(file);
            file.Close();
        }
    }
}
