using Microsoft.EntityFrameworkCore;
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

        public PartituraService(Repository<Partitura> repositoryPartitura, Repository<Genero> repositoryGenero, Repository<Compositor> repositoryCompositor)
        {
            RepositoryPartitura = repositoryPartitura;
            RepositoryGenero = repositoryGenero;
            RepositoryCompositor = repositoryCompositor;
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
        









    }
}
