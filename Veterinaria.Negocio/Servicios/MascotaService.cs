using AutoMapper;
using Veterinaria.Datos.Repositorios;
using Veterinaria.Entidades.Dtos;
using Veterinaria.Entidades.Models;
using Veterinaria.Negocio.Excepciones;

namespace Veterinaria.Negocio.Servicios;

public class MascotaService : IMascotaService
{
    public const int MaxMascotasPorDuenio = 5;

    private readonly IMascotaRepository _repo;
    private readonly IDuenioRepository _duenioRepo;
    private readonly IMapper _mapper;

    public MascotaService(IMascotaRepository repo, IDuenioRepository duenioRepo, IMapper mapper)
    {
        _repo = repo;
        _duenioRepo = duenioRepo;
        _mapper = mapper;
    }

    public async Task<List<MascotaDto>> ObtenerFiltradoAsync(string? especie, int? duenioId, int? edadMin, int? edadMax) =>
        _mapper.Map<List<MascotaDto>>(await _repo.ObtenerFiltradoAsync(especie, duenioId, edadMin, edadMax));

    public async Task<List<MascotaDto>?> ObtenerPorDuenioAsync(int duenioId)
    {
        if (await _duenioRepo.ObtenerPorIdAsync(duenioId) is null) return null;
        return _mapper.Map<List<MascotaDto>>(await _repo.ObtenerPorDuenioAsync(duenioId));
    }

    public async Task<MascotaDto?> ObtenerPorIdAsync(int id)
    {
        var mascota = await _repo.ObtenerPorIdAsync(id);
        return mascota is null ? null : _mapper.Map<MascotaDto>(mascota);
    }

    public async Task<MascotaDto?> CrearAsync(MascotaCrearDto dto)
    {
        if (await _duenioRepo.ObtenerPorIdAsync(dto.DuenioId) is null) return null;

        if (await _repo.ContarPorDuenioAsync(dto.DuenioId) >= MaxMascotasPorDuenio)
            throw new ReglaNegocioException($"Un dueño no puede tener más de {MaxMascotasPorDuenio} mascotas.");

        if (!string.IsNullOrWhiteSpace(dto.Microchip) && await _repo.ExisteMicrochipAsync(dto.Microchip))
            throw new ReglaNegocioException($"Ya existe una mascota con el microchip '{dto.Microchip}'.");

        var mascota = _mapper.Map<Mascota>(dto);
        await _repo.AgregarAsync(mascota);
        var creada = await _repo.ObtenerPorIdAsync(mascota.Id);
        return _mapper.Map<MascotaDto>(creada);
    }

    public async Task<bool> ActualizarAsync(int id, MascotaActualizarDto dto)
    {
        var mascota = await _repo.ObtenerPorIdAsync(id);
        if (mascota is null) return false;

        if (!string.IsNullOrWhiteSpace(dto.Microchip) && await _repo.ExisteMicrochipAsync(dto.Microchip, id))
            throw new ReglaNegocioException($"Ya existe otra mascota con el microchip '{dto.Microchip}'.");

        _mapper.Map(dto, mascota);
        await _repo.GuardarAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var mascota = await _repo.ObtenerPorIdAsync(id);
        if (mascota is null) return false;
        await _repo.EliminarAsync(mascota);
        return true;
    }
}
