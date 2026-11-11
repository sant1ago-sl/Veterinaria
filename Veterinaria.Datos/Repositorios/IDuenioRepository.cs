using Veterinaria.Entidades.Models;

namespace Veterinaria.Datos.Repositorios;

public interface IDuenioRepository
{
    Task<List<Duenio>> ObtenerTodosAsync(string? nombre);
    Task<Duenio?> ObtenerPorIdAsync(int id);
    Task<bool> ExisteEmailAsync(string email, int? excluirId = null);
    Task AgregarAsync(Duenio duenio);
    Task GuardarAsync();
    Task EliminarAsync(Duenio duenio);
}
