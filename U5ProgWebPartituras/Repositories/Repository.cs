using U5ProgWebPartituras.Models.Entities;

namespace U5ProgWebPartituras.Repositories
{
    //Patrón de Repositorio
    //Repository Patterns

    //Genéricos
    //cuando use este repositorio, pongo el tipo = Repository<Partitura>
    public class Repository<T> where T : class //La clase no crea el FruteriashopContext; sino que lo recibe como un parámetro.
    {
        public PartiturasContext Context { get; } //variable publica para usarla en todos los métodos

        public Repository(PartiturasContext context) //Inyección de Dependencias (DI).
        {
            Context = context;
        }
        public IEnumerable<T> GetAll()
        {
            return Context.Set<T>();
        }
        public T? Get(object id)
        {
            return Context.Find<T>(id);
        }
        public void Insert (T entity)
        {
            Context.Add(entity);
            Context.SaveChanges();
        }
        public void Update (T entity)
        {
            Context.Update(entity);
            Context.SaveChanges();
        }
        public void Delete (object id)
        {
            T? entity = Get(id);
            if (entity != null)
            {
                Context.Remove(entity);
                Context.SaveChanges();
            }
        }




    }
}
