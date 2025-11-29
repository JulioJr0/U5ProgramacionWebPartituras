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
            return RepositoryPartitura.GetAll().AsQueryable().Include(x=>x.IdGeneroNavigation).Where(x => x.IdGeneroNavigation != null).Count();
            //return RepositoryPartitura.GetAll().AsQueryable().Include(x => x.IdGenero).Where(x => x.IdGenero == x.IdGeneroNavigation.Id).Count();
        }
        public IndexViewModel GetPartiturasRecientesYXGenero()   //web push para las notificaciones
        {

            IndexViewModel vm = new();
            vm.PartiturasRecientes = RepositoryPartitura.GetAll()
                .AsQueryable()
                .Include(x => x.IdCompositorNavigation)
                .Include(x => x.IdGeneroNavigation)
                .Where(x=>x.IdCompositorNavigation != null)
                .OrderByDescending(x => x.Id)
                .Take(3)
                .Select(x => new PartituraRecienteModel
                {
                    Id = x.Id,
                    Dificultad = x.Dificultad,
                    Titulo = x.Titulo,
                    Descripcion = x.Descripcion,
                    Compositor = x.IdCompositorNavigation.Nombre ?? "compositor desconocido",
                    GeneroMusical = x.IdGeneroNavigation.Nombre ?? "genero musical desconocido"
                }).ToList();

            vm.GenerosMusicales = RepositoryGenero.GetAll()
                .OrderBy(x => x.Nombre)
                .Select(x => new GeneroMusicalModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    NumeroPartituras = RepositoryPartitura.GetAll().Count(p => p.IdGenero == x.Id)
                }).ToList();
            return vm;
        }
        public GeneroViewModel GetPartiturasOrdenadasXGenero(string id)
        {
            var genero = RepositoryGenero.GetAll().Where(x=> x.Nombre == id).FirstOrDefault();
            if (genero != null)
            {
                GeneroViewModel vm = new()
                {
                    Nombre = genero.Nombre,
                    Descripcion = genero.Descripcion,
                    NumeroPartituras = RepositoryPartitura.GetAll().Count(p => p.IdGenero == genero.Id)
                };
                vm.Partituras = RepositoryPartitura.GetAll().AsQueryable().Include(x => x.IdCompositorNavigation).Where(x => x.IdCompositorNavigation !=null && x.IdGeneroNavigation.Nombre == id).OrderBy(x => x.Titulo).Select(x => new PartiturasModel
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
        public DetallesViewModel GetDetallesPartitura(int? id)
        {
            var entidad = RepositoryPartitura.GetAll().AsQueryable().Include(x => x.IdCompositorNavigation).Include(x => x.IdGeneroNavigation)
                .Where(x=> x.Id == id && x.IdCompositorNavigation != null && x.IdGeneroNavigation != null).FirstOrDefault();
            if (entidad != null)
            {
                DetallesViewModel vm = new()
                {
                    Id = id??0,
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
        //index Get
        public IndexAdminPartiturasViewModel GetAllPartituras(int? idFiltroGenero)
        {
            IndexAdminPartiturasViewModel vm = new();
            vm.Generos = RepositoryGenero.GetAll().OrderBy(x => x.Nombre).Select(x => new ItemLista
            {
                Id = x.Id,
                Nombre = x.Nombre
            });
            if (idFiltroGenero != 0 && idFiltroGenero != null) 
            {
                vm.Partituras = RepositoryPartitura.GetAll().AsQueryable().Include(x => x.IdCompositorNavigation).Include(x => x.IdGeneroNavigation)
                .Where(x => x.IdGeneroNavigation != null && x.IdCompositorNavigation != null && x.IdGenero == idFiltroGenero).OrderBy(x => x.Id).Select(x => new PartituraModel
                {
                    Id = x.Id,
                    Titulo = x.Titulo,
                    Compositor = x.IdCompositorNavigation.Nombre,
                    GeneroMusical = x.IdGeneroNavigation.Nombre,
                    Dificultad = x.Dificultad,
                    Instrumentacion = x.Instrumentacion ?? "Instrumento: ?"
                });
            }
            else
            {
                vm.Partituras = RepositoryPartitura.GetAll().AsQueryable().Include(x => x.IdCompositorNavigation).Include(x => x.IdGeneroNavigation)
                .Where(x => x.IdGeneroNavigation != null && x.IdCompositorNavigation != null).OrderBy(x => x.Id).Select(x => new PartituraModel
                {
                    Id = x.Id,
                    Titulo = x.Titulo,
                    Compositor = x.IdCompositorNavigation.Nombre,
                    GeneroMusical = x.IdGeneroNavigation.Nombre,
                    Dificultad = x.Dificultad,
                    Instrumentacion = x.Instrumentacion ?? "Instrumento: ?"
                });
            }
                //||
            //vm.IdGeneroSeleccionado = idFiltroGenero ?? 0;
            return vm;
        }
        //agregar get
        public AgregarAdminPartiturasViewMode GetForDropdowns()
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
        //agregar post
        public int Agregar(AgregarAdminPartiturasViewMode vm)
        {
            try
            {
                var entidad = new Partitura
                {
                    Titulo = vm.Titulo,
                    Instrumentacion = vm.Instrumentacion,
                    Dificultad = vm.Dificultad,
                    Descripcion = vm.Descripcion,
                    IdCompositor = vm.IdCompositor,
                    IdGenero = vm.IdGenero
                };//EF cunado agrega algo, el id va como 0. 

                RepositoryPartitura.Insert(entidad);//antes de estto no sé el id autoincrementable
                return entidad.Id;//el valor es autoincrementable. EF lo trae.
            }
            catch
            {
                return 0;
            }
        }

        //editar get
        public EditarAdminPartiturasViewModel GetForEditar(int id)
        {
            var entidad = RepositoryPartitura.Get(id);
            if (entidad == null)
            {
                throw new ArgumentException("Partitura no encontrada");
            }
            EditarAdminPartiturasViewModel vm = new();

            vm.Id = entidad.Id;
            vm.Titulo = entidad.Titulo;
            vm.IdCompositor = entidad.IdCompositor;
            vm.IdGenero = entidad.IdGenero;
            vm.Instrumentacion = entidad.Instrumentacion?? "Instrumento: ?";
            vm.Dificultad = entidad.Dificultad;
            vm.Descripcion = entidad.Descripcion;

            var dropdowns = GetForDropdowns();
            vm.GenerosLista = dropdowns.GenerosLista;
            vm.CompositoresLista = dropdowns.CompositoresLista;

            return vm;
        }
        //editar post
        public bool Editar(EditarAdminPartiturasViewModel vm)
        {
            try
            {
                var entidad = RepositoryPartitura.Get(vm.Id);
                if (entidad == null) { throw new ArgumentException("Partitura no existe."); }

                entidad.Titulo = vm.Titulo;
                entidad.IdCompositor = vm.IdCompositor;
                entidad.IdGenero = vm.IdGenero;
                entidad.Instrumentacion = vm.Instrumentacion;
                entidad.Dificultad = vm.Dificultad;
                entidad.Descripcion = vm.Descripcion;

                RepositoryPartitura.Update(entidad);
                return true;
            }
            catch
            {
                return false;
            }
            
            //en el controlador me traeré los generos y partituras 
        }
        //eliminar get
        public EliminarAdminPartiturasViewModel GetForEliminar(int id)
        {
            var entidad = RepositoryPartitura.Get(id);
            if (entidad == null) { throw new ArgumentException("Partitura no existe"); }

            return new EliminarAdminPartiturasViewModel
            {
                Id = id,
                Nombre = entidad.Titulo
            };
        }
        public bool Eliminar(int id)
        {
            try
            {
                var entidad = RepositoryPartitura.Get(id);
                if (entidad == null) return false;

                RepositoryPartitura.Delete(entidad.Id);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
