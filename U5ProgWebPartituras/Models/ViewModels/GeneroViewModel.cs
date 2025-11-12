namespace U5ProgWebPartituras.Models.ViewModels
{
    public class GeneroViewModel
    {
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public int NumeroPartituras { get; set; }
        public IEnumerable<PartiturasModel> Partituras { get; set; } = null!;

    }
        public class PartiturasModel
        {
            public int Id { get; set; }
            public string? Dificultad { get; set; }
            public string Titulo { get; set; } = null!;
            public string Compositor { get; set; } = null!;
            public string Instrumentacion { get; set; } = null!;
            public string? Descripcion { get; set; }
        }
}


