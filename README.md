# Veterinaria API (Semana 13)

Capas: Entidades → Datos → Negocio → Api. Solución: `Veterinaria.slnx`. BD: ejecutar `Database/VeterinariaDb.sql` (LocalDB: `(localdb)\MSSQLLocalDB`).
Ejecutar: `dotnet run --project Veterinaria.Api --launch-profile http` → http://localhost:5208/swagger

## Endpoints
Duenios: GET /api/duenios?nombre= · GET /{id} · GET /{id}/mascotas (anidada) · POST · PUT /{id} · DELETE /{id}
Mascotas: GET /api/mascotas?especie=&duenioId=&edadMin=&edadMax= · GET /{id} · POST · PUT /{id} · DELETE /{id}

## Reglas de negocio (→ 409)
- Email de dueño único; no eliminar dueño con mascotas.
- Máximo 5 mascotas por dueño; microchip único.

## Mapeos (MappingProfile)
| Mapeo | Origen → Destino | ¿Por qué en ese sentido? | Configuración especial |
|---|---|---|---|
| Duenio → DuenioDto | Entidad → DTO lectura | Es lo que la API devuelve; la entidad no sale | ForMember: TotalMascotas = Mascotas.Count |
| Mascota → MascotaDto | Entidad → DTO lectura | Idem | ForMember: DuenioNombre = Duenio.Nombre |
| DuenioCrearDto → Duenio | DTO entrada → Entidad | Llega del cliente y se guarda | Ninguna (FechaRegistro la fija el servicio) |
| DuenioActualizarDto → Duenio | DTO entrada → Entidad | Map(origen, destino) sobre la entidad existente | Ninguna |
| MascotaCrearDto → Mascota | DTO entrada → Entidad | Llega del cliente y se guarda | Ninguna |
| MascotaActualizarDto → Mascota | DTO entrada → Entidad | Map(origen, destino); no permite cambiar de dueño | Ninguna |
No se usa ReverseMap(): los DTOs de entrada y lectura son distintos, un mapeo inverso no tendría sentido.
