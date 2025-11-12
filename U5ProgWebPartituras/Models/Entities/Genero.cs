using System;
using System.Collections.Generic;

namespace U5ProgWebPartituras.Models.Entities;

public partial class Genero
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Partitura> Partitura { get; set; } = new List<Partitura>();
}
