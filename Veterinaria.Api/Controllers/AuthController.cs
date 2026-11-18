using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Veterinaria.Entidades.Dtos;
using Veterinaria.Negocio.Excepciones;
using Veterinaria.Negocio.Servicios;

namespace Veterinaria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service) => _service = service;

    [AllowAnonymous]
    [HttpPost("registro")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Registro(RegistroDto dto)
    {
        try
        {
            var creado = await _service.RegistrarAsync(dto);
            return StatusCode(StatusCodes.Status201Created, creado);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new ProblemDetails { Title = "Regla de negocio incumplida", Detail = ex.Message, Status = 409 });
        }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var token = await _service.LoginAsync(dto);
        return token is null ? Unauthorized() : Ok(token);
    }

    [Authorize]
    [HttpGet("perfil")]
    [ProducesResponseType(typeof(PerfilDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Perfil()
    {
        var correo = User.FindFirst("email")?.Value ?? User.FindFirst(ClaimTypes.Email)?.Value;
        var rol = User.FindFirst("role")?.Value ?? User.FindFirst(ClaimTypes.Role)?.Value;
        return Ok(new PerfilDto { Correo = correo ?? string.Empty, Rol = rol ?? string.Empty });
    }
}
