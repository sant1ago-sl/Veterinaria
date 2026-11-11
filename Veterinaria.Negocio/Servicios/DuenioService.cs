using AutoMapper;
using Veterinaria.Datos.Repositorios;
using Veterinaria.Entidades.Dtos;
using Veterinaria.Entidades.Models;
using Veterinaria.Negocio.Excepciones;

namespace Veterinaria.Negocio.Servicios;

public class DuenioService : IDuenioService
{
    private readonly IDuenioRepository _repo;
    private readonly IMapper _mapper;

    public DuenioService(IDuenioRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<List<DuenioDto>> ObtenerTodosAsync(string? nombre) =>
        _mapper.Map<List<DuenioDto>>(await _repo.ObtenerTodosAsync(nombre));

    public async Task<DuenioDto?> ObtenerPorIdAsync(int id)
    {
        var duenio = await _repo.ObtenerPorIdAsync(id);
        return duenio is null ? null : _mapper.Map<DuenioDto>(duenio);
    }

    public async Task<DuenioDto> CrearAsync(DuenioCrearDto dto)
    {
        // Regla 1: el email del dueño debe ser único
        if (await _repo.ExisteEmailAsync(dto.Email))
            throw new ReglaNegocioException($"Ya existe un dueño con el email '{dto.Email}'.");

        var duenio = _mapper.Map<Duenio>(dto);
        duenio.FechaRegistro = DateTime.UtcNow;
        await _repo.AgregarAsync(duenio);
        return _mapper.Map<DuenioDto>(duenio);
    }

    public async Task<bool> ActualizarAsync(int id, DuenioActualizarDto dto)
    {
        var duenio = await _repo.ObtenerPorIdAsync(id);
        if (duenio is null) return false;

        if (await _repo.ExisteEmailAsync(dto.Email, id))
            throw new ReglaNegocioException($"Ya existe otro dueño con el email '{dto.Email}'.");

        _mapper.Map(dto, duenio);
        await _repo.GuardarAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var duenio = await _repo.ObtenerPorIdAsync(id);
        if (duenio is null) return false;

        // Regla 2: no se puede eliminar un dueño con mascotas registradas
        if (duenio.Mascotas.Any())
            throw new ReglaNegocioException("No se puede eliminar un dueño que tiene mascotas registradas.");

        await _repo.EliminarAsync(duenio);
        return true;
    }
}
