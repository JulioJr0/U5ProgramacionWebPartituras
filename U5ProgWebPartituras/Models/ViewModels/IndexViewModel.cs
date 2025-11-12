namespace U5ProgWebPartituras.Models.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<PartituraRecienteModel> PartiturasRecientes { get; set; } = null!;
        public IEnumerable<GeneroMusicalModel> GenerosMusicales { get; set; } = null!;
    }
        public class PartituraRecienteModel
        {
            public int Id { get; set; }
            public string? Dificultad { get; set; }
            public string Titulo { get; set; } = null!;
            public string Compositor { get; set; } = null!;
            public string? Descripcion { get; set; }
            public string GeneroMusical { get; set; } = null!;
        }
        public class GeneroMusicalModel
        {
            public int Id { get; set; }
            public string Nombre { get; set; } = null!;
            public string? Descripcion { get; set; }
            public int NumeroPartituras { get; set; }
        }



}