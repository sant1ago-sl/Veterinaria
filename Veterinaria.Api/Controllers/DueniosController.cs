using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Veterinaria.Entidades.Dtos;
using Veterinaria.Negocio.Excepciones;
using Veterinaria.Negocio.Servicios;

namespace Veterinaria.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class DueniosController : ControllerBase
{
    private readonly IDuenioService _service;
    private readonly IMascotaService _mascotaService;

    public DueniosController(IDuenioService service, IMascotaService mascotaService)
    {
        _service = service;
        _mascotaService = mascotaService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(List<DuenioDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] string? nombre) =>
        Ok(await _service.ObtenerTodosAsync(nombre));

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(DuenioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var duenio = await _service.ObtenerPorIdAsync(id);
        return duenio is null ? NotFound() : Ok(duenio);
    }

    [HttpGet("{id:int}/mascotas")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(List<MascotaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMascotas(int id)
    {
        var mascotas = await _mascotaService.ObtenerPorDuenioAsync(id);
        return mascotas is null ? NotFound() : Ok(mascotas);
    }

    [Authorize(Roles = AuthService.RolRecepcionista)]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(DuenioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Post(DuenioCrearDto dto)
    {
        try
        {
            var creado = await _service.CrearAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new ProblemDetails { Title = "Regla de negocio incumplida", Detail = ex.Message, Status = 409 });
        }
    }

    [Authorize(Roles = AuthService.RolRecepcionista)]
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Put(int id, DuenioActualizarDto dto)
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

    [Authorize(Roles = AuthService.RolRecepcionista)]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            return await _service.EliminarAsync(id) ? NoContent() : NotFound();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new ProblemDetails { Title = "Regla de negocio incumplida", Detail = ex.Message, Status = 409 });
        }
    }
}
