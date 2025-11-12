using System;
using System.Collections.Generic;

namespace U5ProgWebPartituras.Models.Entities;

public partial class Partitura
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Instrumentacion { get; set; }

    /// <summary>
    /// Ej: Fácil, Intermedio, Avanzado
    /// </summary>
    public string? Dificultad { get; set; }

    public string? Descripcion { get; set; }

    public int IdGenero { get; set; }

    public int IdCompositor { get; set; }

    public virtual Compositor IdCompositorNavigation { get; set; } = null!;

    public virtual Genero IdGeneroNavigation { get; set; } = null!;
}
