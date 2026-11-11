using Microsoft.EntityFrameworkCore;
using Veterinaria.Entidades.Models;

namespace Veterinaria.Datos.Repositorios;

public class DuenioRepository : IDuenioRepository
{
    private readonly VeterinariaContext _context;
    public DuenioRepository(VeterinariaContext context) => _context = context;

    public async Task<List<Duenio>> ObtenerTodosAsync(string? nombre)
    {
        var query = _context.Duenios.Include(d => d.Mascotas).AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(nombre))
            query = query.Where(d => d.Nombre.Contains(nombre));
        return await query.OrderBy(d => d.Nombre).ToListAsync();
    }

    public async Task<Duenio?> ObtenerPorIdAsync(int id) =>
        await _context.Duenios.Include(d => d.Mascotas).FirstOrDefaultAsync(d => d.Id == id);

    public async Task<bool> ExisteEmailAsync(string email, int? excluirId = null) =>
        await _context.Duenios.AnyAsync(d => d.Email == email && (excluirId == null || d.Id != excluirId));

    public async Task AgregarAsync(Duenio duenio)
    {
        await _context.Duenios.AddAsync(duenio);
        await _context.SaveChangesAsync();
    }

    public async Task GuardarAsync() => await _context.SaveChangesAsync();

    public async Task EliminarAsync(Duenio duenio)
    {
        _context.Duenios.Remove(duenio);
        await _context.SaveChangesAsync();
    }
}
