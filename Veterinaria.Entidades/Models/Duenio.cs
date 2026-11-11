using System;
using System.Collections.Generic;

namespace Veterinaria.Entidades.Models;

public partial class Duenio
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Telefono { get; set; }

    public DateTime FechaRegistro { get; set; }

    public virtual ICollection<Mascota> Mascotas { get; set; } = new List<Mascota>();
}
