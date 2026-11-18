using Veterinaria.Entidades.Models;

namespace Veterinaria.Datos.Repositorios;

public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorCorreoAsync(string correo);
    Task AgregarAsync(Usuario usuario);
    Task GuardarAsync();
}
