using Veterinaria.Entidades.Dtos;

namespace Veterinaria.Negocio.Servicios;

public interface IMascotaService
{
    Task<List<MascotaDto>> ObtenerFiltradoAsync(string? especie, int? duenioId, int? edadMin, int? edadMax);
    Task<List<MascotaDto>?> ObtenerPorDuenioAsync(int duenioId);
    Task<MascotaDto?> ObtenerPorIdAsync(int id);
    Task<MascotaDto?> CrearAsync(MascotaCrearDto dto);
    Task<bool> ActualizarAsync(int id, MascotaActualizarDto dto);
    Task<bool> EliminarAsync(int id);
}
