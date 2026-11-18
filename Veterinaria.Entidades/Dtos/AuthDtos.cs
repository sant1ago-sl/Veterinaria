using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Entidades.Dtos;

public class LoginDto
{
    [Required, EmailAddress, StringLength(150)]
    public string Correo { get; set; } = null!;

    [Required, StringLength(100)]
    public string Clave { get; set; } = null!;
}

public class RegistroDto
{
    [Required, EmailAddress, StringLength(150)]
    public string Correo { get; set; } = null!;

    [Required, StringLength(100, MinimumLength = 8)]
    public string Clave { get; set; } = null!;
}

public class TokenDto
{
    public string Token { get; set; } = null!;
    public DateTime Expiracion { get; set; }
}

public class UsuarioDto
{
    public int Id { get; set; }
    public string Correo { get; set; } = null!;
    public string Rol { get; set; } = null!;
}

public class PerfilDto
{
    public string Correo { get; set; } = null!;
    public string Rol { get; set; } = null!;
}
