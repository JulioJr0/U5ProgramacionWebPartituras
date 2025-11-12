using System;
using System.Collections.Generic;

namespace U5ProgWebPartituras.Models.Entities;

public partial class Compositor
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Nacionalidad { get; set; }

    public string? Biografia { get; set; }

    public virtual ICollection<Partitura> Partitura { get; set; } = new List<Partitura>();
}
