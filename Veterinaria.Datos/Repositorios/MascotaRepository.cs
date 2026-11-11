using Microsoft.EntityFrameworkCore;
using Veterinaria.Entidades.Models;

namespace Veterinaria.Datos.Repositorios;

public class MascotaRepository : IMascotaRepository
{
    private readonly VeterinariaContext _context;
    public MascotaRepository(VeterinariaContext context) => _context = context;

    public async Task<List<Mascota>> ObtenerFiltradoAsync(string? especie, int? duenioId, int? edadMin, int? edadMax)
    {
        var query = _context.Mascotas.Include(m => m.Duenio).AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(especie)) query = query.Where(m => m.Especie == especie);
        if (duenioId.HasValue) query = query.Where(m => m.DuenioId == duenioId.Value);
        if (edadMin.HasValue) query = query.Where(m => m.Edad >= edadMin.Value);
        if (edadMax.HasValue) query = query.Where(m => m.Edad <= edadMax.Value);
        return await query.OrderBy(m => m.Nombre).ToListAsync();
    }

    public async Task<Mascota?> ObtenerPorIdAsync(int id) =>
        await _context.Mascotas.Include(m => m.Duenio).FirstOrDefaultAsync(m => m.Id == id);

    public async Task<List<Mascota>> ObtenerPorDuenioAsync(int duenioId) =>
        await _context.Mascotas.Include(m => m.Duenio).AsNoTracking()
            .Where(m => m.DuenioId == duenioId).OrderBy(m => m.Nombre).ToListAsync();

    public async Task<int> ContarPorDuenioAsync(int duenioId) =>
        await _context.Mascotas.CountAsync(m => m.DuenioId == duenioId);

    public async Task<bool> ExisteMicrochipAsync(string microchip, int? excluirId = null) =>
        await _context.Mascotas.AnyAsync(m => m.Microchip == microchip && (excluirId == null || m.Id != excluirId));

    public async Task AgregarAsync(Mascota mascota)
    {
        await _context.Mascotas.AddAsync(mascota);
        await _context.SaveChangesAsync();
    }

    public async Task GuardarAsync() => await _context.SaveChangesAsync();

    public async Task EliminarAsync(Mascota mascota)
    {
        _context.Mascotas.Remove(mascota);
        await _context.SaveChangesAsync();
    }
}
