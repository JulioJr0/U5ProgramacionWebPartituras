namespace U5ProgWebPartituras.Areas.Admin.Models
{
    public class IndexAdminPartiturasViewModel
    {
        public IEnumerable<PartituraModel> Partituras { get; set; } = null!;
        public IEnumerable<ItemLista> Generos { get; set; } = null!;
        public int IdGeneroSeleccionado { get; set; }
    }
        public class ItemLista
        {
            public int Id { get; set; }
            public string Nombre { get; set; } = null!;
        }
        public class PartituraModel
        {
            public int Id { get; set; }
            public string Titulo { get; set; } = null!;
            public string Compositor { get; set; } = null!;
            public string GeneroMusical { get; set; } = null!;
            public string? Dificultad { get; set; }
            public string Instrumentacion { get; set; } = null!;
        }

    public class AgregarAdminPartiturasViewMode
    {
        public string Titulo { get; set; } = null!;
        public IEnumerable<ItemLista>? CompositoresLista {  get; set; }
        public int IdCompositor { get; set; }
        public IEnumerable<ItemLista>? GenerosLista { get; set; }
        public int IdGenero {  get; set; }
        public string Instrumentacion { get; set; } = null!;
        public string? Dificultad { get; set; }
        public string? Descripcion { get; set; }
        public IFormFile Pdf { get; set; } = null!;
        public IFormFile? Audio { get; set; }
    }

    public class EditarAdminPartiturasViewModel:AgregarAdminPartiturasViewMode
    {
        public int Id {  get; set; }
    }

    public class EliminarAdminPartiturasViewModel:ItemLista
    {

    }
}
