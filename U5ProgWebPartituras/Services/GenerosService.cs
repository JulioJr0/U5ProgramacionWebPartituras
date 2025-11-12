using U5ProgWebPartituras.Models.Entities;
using U5ProgWebPartituras.Repositories;

namespace U5ProgWebPartituras.Services
{
    public class GenerosService
    {
        public Repository<Genero> GeneroRepository { get; }

        public GenerosService(Repository<Genero> generoRepository)
        {
            GeneroRepository = generoRepository;
        }
        //Para el filtro
        public IEnumerable<string> ListaGeneros()
        {
            return GeneroRepository.GetAll().OrderBy(x=> x.Id).Select(x=> x.Nombre);
        }




    }
}
