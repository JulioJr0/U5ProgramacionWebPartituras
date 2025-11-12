namespace U5ProgWebPartituras.Models.ViewModels
{
    public class DetallesViewModel:PartiturasModel
    {
        public string GeneroMusical { get; set; } = null!;
        public string? Nacionalidad { get; set; }
        public string? Biografia { get; set; } = null!;

    }
}