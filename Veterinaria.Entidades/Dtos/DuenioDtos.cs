using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Entidades.Dtos;

public class DuenioDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Telefono { get; set; }
    public DateTime FechaRegistro { get; set; }
    public int TotalMascotas { get; set; }
}

public class DuenioCrearDto
{
    [Required, StringLength(100, MinimumLength = 2)]
    public string Nombre { get; set; } = null!;

    [Required, EmailAddress, StringLength(150)]
    public string Email { get; set; } = null!;

    [StringLength(20)]
    public string? Telefono { get; set; }
}

public class DuenioActualizarDto
{
    [Required, StringLength(100, MinimumLength = 2)]
    public string Nombre { get; set; } = null!;

    [Required, EmailAddress, StringLength(150)]
    public string Email { get; set; } = null!;

    [StringLength(20)]
    public string? Telefono { get; set; }
}
