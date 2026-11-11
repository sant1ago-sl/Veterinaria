using Veterinaria.Entidades.Dtos;

namespace Veterinaria.Negocio.Servicios;

public interface IDuenioService
{
    Task<List<DuenioDto>> ObtenerTodosAsync(string? nombre);
    Task<DuenioDto?> ObtenerPorIdAsync(int id);
    Task<DuenioDto> CrearAsync(DuenioCrearDto dto);
    Task<bool> ActualizarAsync(int id, DuenioActualizarDto dto);
    Task<bool> EliminarAsync(int id);
}
