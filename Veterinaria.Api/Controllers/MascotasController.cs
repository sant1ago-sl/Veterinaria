using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Veterinaria.Entidades.Dtos;
using Veterinaria.Negocio.Excepciones;
using Veterinaria.Negocio.Servicios;

namespace Veterinaria.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class MascotasController : ControllerBase
{
    private readonly IMascotaService _service;

    public MascotasController(IMascotaService service) => _service = service;

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(List<MascotaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] string? especie, [FromQuery] int? duenioId,
        [FromQuery] int? edadMin, [FromQuery] int? edadMax) =>
        Ok(await _service.ObtenerFiltradoAsync(especie, duenioId, edadMin, edadMax));

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(MascotaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var mascota = await _service.ObtenerPorIdAsync(id);
        return mascota is null ? NotFound() : Ok(mascota);
    }

    [Authorize(Roles = AuthService.RolRecepcionista + "," + AuthService.RolVeterinario)]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(MascotaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Post(MascotaCrearDto dto)
    {
        try
        {
            var creada = await _service.CrearAsync(dto);
            if (creada is null) return NotFound(new ProblemDetails { Title = "Dueño no encontrado", Status = 404 });
            return CreatedAtAction(nameof(GetById), new { id = creada.Id }, creada);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new ProblemDetails { Title = "Regla de negocio incumplida", Detail = ex.Message, Status = 409 });
        }
    }

    [Authorize(Roles = AuthService.RolVeterinario)]
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Put(int id, MascotaActualizarDto dto)
    {
        try
        {
            return await _service.ActualizarAsync(id, dto) ? NoContent() : NotFound();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new ProblemDetails { Title = "Regla de negocio incumplida", Detail = ex.Message, Status = 409 });
        }
    }

    [Authorize(Roles = AuthService.RolVeterinario)]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id) =>
        await _service.EliminarAsync(id) ? NoContent() : NotFound();
}
