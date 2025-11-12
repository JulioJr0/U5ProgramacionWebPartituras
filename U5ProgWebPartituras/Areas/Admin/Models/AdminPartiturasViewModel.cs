namespace U5ProgWebPartituras.Areas.Admin.Models
{
    public class IndexAdminPartiturasViewModel
    {
        public IEnumerable<PartituraModel> Partituras { get; set; } = null!;
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

    public class AgregarAdminPartiturasViewMode:PartituraModel
    {
        public string? Descripcion { get; set; }
        public IFormFile Pdf { get; set; } = null!;
        public IFormFile? Audio { get; set; }
    }

    public class EditarAdminPartiturasViewModel:AgregarAdminPartiturasViewMode
    {
    }

    public class EliminarAdminPartiturasViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set;} = null!;

    }
}
