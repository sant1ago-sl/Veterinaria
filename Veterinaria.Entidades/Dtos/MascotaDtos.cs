using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Entidades.Dtos;

public class MascotaDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Especie { get; set; } = null!;
    public string? Raza { get; set; }
    public int Edad { get; set; }
    public decimal Peso { get; set; }
    public string? Microchip { get; set; }
    public int DuenioId { get; set; }
    public string DuenioNombre { get; set; } = null!;
}

public class MascotaCrearDto
{
    [Required, StringLength(80, MinimumLength = 1)]
    public string Nombre { get; set; } = null!;

    [Required, StringLength(50)]
    public string Especie { get; set; } = null!;

    [StringLength(80)]
    public string? Raza { get; set; }

    [Range(0, 50)]
    public int Edad { get; set; }

    [Range(0.01, 999.99)]
    public decimal Peso { get; set; }

    [StringLength(30)]
    public string? Microchip { get; set; }

    [Range(1, int.MaxValue)]
    public int DuenioId { get; set; }
}

public class MascotaActualizarDto
{
    [Required, StringLength(80, MinimumLength = 1)]
    public string Nombre { get; set; } = null!;

    [Required, StringLength(50)]
    public string Especie { get; set; } = null!;

    [StringLength(80)]
    public string? Raza { get; set; }

    [Range(0, 50)]
    public int Edad { get; set; }

    [Range(0.01, 999.99)]
    public decimal Peso { get; set; }

    [StringLength(30)]
    public string? Microchip { get; set; }
}
