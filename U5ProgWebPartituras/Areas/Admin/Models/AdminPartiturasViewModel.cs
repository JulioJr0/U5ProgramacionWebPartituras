using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;

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
        [Required(ErrorMessage = "Escribe el título de la partitura.")]
        [StringLength(150, ErrorMessage = "El título no puede exceder 150 caracteres.")]
        public string Titulo { get; set; } = null!;

        public IEnumerable<ItemLista>? CompositoresLista {  get; set; }

        [Required(ErrorMessage = "Selecciona un compositor.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un compositor válido.")]
        public int IdCompositor { get; set; }

        public IEnumerable<ItemLista>? GenerosLista { get; set; }

        [Required(ErrorMessage = "Selecciona un género musical.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un género válido.")]
        public int IdGenero {  get; set; }

        [Required(ErrorMessage = "Escribe que tipo de instrumentos se utilizan en la partitura (Piano solo, Violín solo, Guitarra  Clásica sola, Piano y violín, etc).")]
        [StringLength(150, ErrorMessage = "La instrumentación no puede exceder 150 caracteres.")]
        public string Instrumentacion { get; set; } = null!;

        [StringLength(50, ErrorMessage = "La dificultad no puede exceder 50 caracteres.")]
        public string? Dificultad { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres.")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "Debe subir un archivo PDF.")]
        [DataType(DataType.Upload)]
        public IFormFile Pdf { get; set; } = null!;

        [DataType(DataType.Upload)]
        public IFormFile? Audio { get; set; }
    }

    public class EditarAdminPartiturasViewModel:AgregarAdminPartiturasViewMode
    {
        public int Id {  get; set; }
        [DataType(DataType.Upload)]
        public new IFormFile? Pdf { get; set; }
    }

    public class EliminarAdminPartiturasViewModel:ItemLista
    {

    }
}


//< div class= "form-group" >
//    < label > Título </ label >
//    @Html.TextBoxFor(m => m.Titulo, new { @class = "form-control" })
//    @Html.ValidationMessageFor(m => m.Titulo, "", new { @class = "text-danger" })
//</ div >

//@Html.ValidationMessageFor(
//    expression: model => model.NombreDelCampo,
//    message: "",                                  // mensaje por defecto del modelo
//    htmlAttributes: new { @class = "text-danger" } // clases CSS opcionales
//)

//< div class= "form-group" >
//    < label for= "Titulo" > Título </ label >
//    @Html.TextBoxFor(m => m.Titulo, new { @class = "form-control" })
//    @Html.ValidationMessageFor(m => m.Titulo, "", new { @class = "text-danger" })
//</ div >