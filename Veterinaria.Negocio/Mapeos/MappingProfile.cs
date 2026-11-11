using AutoMapper;
using Veterinaria.Entidades.Dtos;
using Veterinaria.Entidades.Models;

namespace Veterinaria.Negocio.Mapeos;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Duenio, DuenioDto>()
            .ForMember(d => d.TotalMascotas, o => o.MapFrom(s => s.Mascotas.Count));
        CreateMap<Mascota, MascotaDto>()
            .ForMember(d => d.DuenioNombre, o => o.MapFrom(s => s.Duenio.Nombre));

        CreateMap<DuenioCrearDto, Duenio>();
        CreateMap<DuenioActualizarDto, Duenio>();
        CreateMap<MascotaCrearDto, Mascota>();
        CreateMap<MascotaActualizarDto, Mascota>();
    }
}
