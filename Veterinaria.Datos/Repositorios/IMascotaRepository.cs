using Veterinaria.Entidades.Models;

namespace Veterinaria.Datos.Repositorios;

public interface IMascotaRepository
{
    Task<List<Mascota>> ObtenerFiltradoAsync(string? especie, int? duenioId, int? edadMin, int? edadMax);
    Task<Mascota?> ObtenerPorIdAsync(int id);
    Task<List<Mascota>> ObtenerPorDuenioAsync(int duenioId);
    Task<int> ContarPorDuenioAsync(int duenioId);
    Task<bool> ExisteMicrochipAsync(string microchip, int? excluirId = null);
    Task AgregarAsync(Mascota mascota);
    Task GuardarAsync();
    Task EliminarAsync(Mascota mascota);
}
