# Veterinaria API (Semana 13)

API REST en **ASP.NET Core (.NET 10)** para gestionar dueños y mascotas de una veterinaria, construida en **4 capas** con Entity Framework Core, AutoMapper y SQL Server.

---

<a id="indice"></a>
## Índice

1. [Enlace del repositorio](#enlace)
2. [Script SQL de la base de datos](#script-sql)
3. [Arquitectura en capas](#arquitectura)
4. [Modelo de datos](#modelo-datos)
5. [Configuración y ejecución](#ejecucion)
6. [Endpoints](#endpoints)
   - [Dueños](#endpoints-duenios)
   - [Mascotas](#endpoints-mascotas)
7. [Reglas de negocio](#reglas-negocio)
8. [Mapeos (MappingProfile)](#mapeos)
9. [Seguridad: sin credenciales en el repositorio](#seguridad)

---

<a id="enlace"></a>
## 1. Enlace del repositorio

**https://github.com/sant1ago-sl/Veterinaria.git**

- Repositorio: [github.com/sant1ago-sl/Veterinaria](https://github.com/sant1ago-sl/Veterinaria)
- Rama principal: `master`
- Solución a abrir: [`Veterinaria.slnx`](Veterinaria.slnx)

El repositorio no contiene contraseñas ni cadenas de conexión con credenciales (ver [sección 9](#seguridad)).

[↑ Volver al índice](#indice)

---

<a id="script-sql"></a>
## 2. Script SQL de la base de datos

El script está incluido en el repositorio: [`Database/VeterinariaDb.sql`](Database/VeterinariaDb.sql)

Qué hace:

- Crea la base de datos `VeterinariaDb` si no existe.
- Crea las tablas `Duenios` y `Mascotas` con sus claves primarias, clave foránea, restricciones `CHECK` y los índices únicos.
- Inserta datos de ejemplo: 3 dueños y 4 mascotas.

Cómo ejecutarlo:

1. Abre SQL Server Management Studio, Azure Data Studio o la extensión de SQL de Visual Studio.
2. Conéctate a tu instancia local (por ejemplo, LocalDB: `(localdb)\MSSQLLocalDB`).
3. Abre `Database/VeterinariaDb.sql` y ejecútalo completo.

> Nota: el script elimina y vuelve a crear las tablas `Mascotas` y `Duenios`, por lo que reinicia los datos cada vez que se ejecuta.

[↑ Volver al índice](#indice)

---

<a id="arquitectura"></a>
## 3. Arquitectura en capas

Flujo de dependencias: **Entidades → Datos → Negocio → Api**.

| Capa | Proyecto | Responsabilidad |
|---|---|---|
| Entidades | [`Veterinaria.Entidades`](Veterinaria.Entidades) | Modelos de la base de datos (`Models`) y DTOs de entrada y salida (`Dtos`). |
| Datos | [`Veterinaria.Datos`](Veterinaria.Datos) | `VeterinariaContext` (EF Core) y repositorios (`Repositorios`). |
| Negocio | [`Veterinaria.Negocio`](Veterinaria.Negocio) | Servicios con las reglas de negocio, `MappingProfile` y `ReglaNegocioException`. |
| Api | [`Veterinaria.Api`](Veterinaria.Api) | Controladores REST, configuración, inyección de dependencias y Swagger. |

```
Veterinaria/
├── Database/
│   └── VeterinariaDb.sql
├── Veterinaria.Entidades/    (Models, Dtos)
├── Veterinaria.Datos/        (VeterinariaContext, Repositorios)
├── Veterinaria.Negocio/      (Servicios, Mapeos, Excepciones)
├── Veterinaria.Api/          (Controllers, Program.cs)
└── Veterinaria.slnx
```

[↑ Volver al índice](#indice)

---

<a id="modelo-datos"></a>
## 4. Modelo de datos

Relación: un **dueño** tiene muchas **mascotas** (1:N).

**Duenios**

| Columna | Tipo | Restricciones |
|---|---|---|
| Id | INT | PK, identidad |
| Nombre | NVARCHAR(100) | Obligatorio |
| Email | NVARCHAR(150) | Obligatorio, único |
| Telefono | NVARCHAR(20) | Opcional |
| FechaRegistro | DATETIME2 | Obligatorio, por defecto la fecha UTC actual |

**Mascotas**

| Columna | Tipo | Restricciones |
|---|---|---|
| Id | INT | PK, identidad |
| Nombre | NVARCHAR(80) | Obligatorio |
| Especie | NVARCHAR(50) | Obligatorio |
| Raza | NVARCHAR(80) | Opcional |
| Edad | INT | Obligatorio, entre 0 y 50 |
| Peso | DECIMAL(6,2) | Obligatorio, mayor que 0 |
| Microchip | NVARCHAR(30) | Opcional, único cuando no es nulo |
| DuenioId | INT | FK a `Duenios(Id)` |

[↑ Volver al índice](#indice)

---

<a id="ejecucion"></a>
## 5. Configuración y ejecución

**Requisitos:** .NET 10 SDK, SQL Server o LocalDB y Visual Studio 2022 o superior (opcional).

1. Clona el repositorio:
   ```bash
   git clone https://github.com/sant1ago-sl/Veterinaria.git
   ```
2. Ejecuta el script [`Database/VeterinariaDb.sql`](#script-sql).
3. Configura tu cadena de conexión **en tu máquina** con User Secrets, sin guardarla en el repositorio (ver [sección 9](#seguridad)).
4. Ejecuta la API:
   ```bash
   dotnet run --project Veterinaria.Api --launch-profile http
   ```
5. Abre Swagger en http://localhost:5208/swagger

**Desde Visual Studio:** abre `Veterinaria.slnx` (no `web_sem_13.slnx`), establece `Veterinaria.Api` como proyecto de inicio, elige el perfil `http` o `https` y pulsa F5.

[↑ Volver al índice](#indice)

---

<a id="endpoints"></a>
## 6. Endpoints

Todos los endpoints se pueden probar desde Swagger en `/swagger`.

<a id="endpoints-duenios"></a>
### Dueños (`/api/duenios`)

| Método | Ruta | Descripción | Respuestas |
|---|---|---|---|
| GET | `/api/duenios?nombre=` | Lista dueños, con filtro opcional por nombre | 200 |
| GET | `/api/duenios/{id}` | Obtiene un dueño | 200, 404 |
| GET | `/api/duenios/{id}/mascotas` | Mascotas de un dueño (ruta anidada) | 200, 404 |
| POST | `/api/duenios` | Crea un dueño | 201, 400, 409 |
| PUT | `/api/duenios/{id}` | Actualiza un dueño | 204, 400, 404, 409 |
| DELETE | `/api/duenios/{id}` | Elimina un dueño | 204, 404, 409 |

<a id="endpoints-mascotas"></a>
### Mascotas (`/api/mascotas`)

| Método | Ruta | Descripción | Respuestas |
|---|---|---|---|
| GET | `/api/mascotas?especie=&duenioId=&edadMin=&edadMax=` | Lista mascotas con filtros opcionales | 200 |
| GET | `/api/mascotas/{id}` | Obtiene una mascota | 200, 404 |
| POST | `/api/mascotas` | Crea una mascota | 201, 400, 404, 409 |
| PUT | `/api/mascotas/{id}` | Actualiza una mascota | 204, 400, 404, 409 |
| DELETE | `/api/mascotas/{id}` | Elimina una mascota | 204, 404 |

[↑ Volver al índice](#indice)

---

<a id="reglas-negocio"></a>
## 7. Reglas de negocio

Cuando una regla no se cumple, la API responde **409 Conflict** con un `ProblemDetails`.

- El email de un dueño es único.
- No se puede eliminar un dueño que tiene mascotas registradas.
- Un dueño puede tener como máximo 5 mascotas.
- El microchip de una mascota es único.

Implementación: [`DuenioService`](Veterinaria.Negocio/Servicios/DuenioService.cs) y [`MascotaService`](Veterinaria.Negocio/Servicios/MascotaService.cs).

[↑ Volver al índice](#indice)

---

<a id="mapeos"></a>
## 8. Mapeos (MappingProfile)

Archivo: [`Veterinaria.Negocio/Mapeos/MappingProfile.cs`](Veterinaria.Negocio/Mapeos/MappingProfile.cs)

| Mapeo | Origen → Destino | ¿Por qué en ese sentido? | Configuración especial |
|---|---|---|---|
| Duenio → DuenioDto | Entidad → DTO lectura | Es lo que la API devuelve; la entidad no sale | ForMember: TotalMascotas = Mascotas.Count |
| Mascota → MascotaDto | Entidad → DTO lectura | Idem | ForMember: DuenioNombre = Duenio.Nombre |
| DuenioCrearDto → Duenio | DTO entrada → Entidad | Llega del cliente y se guarda | Ninguna (FechaRegistro la fija el servicio) |
| DuenioActualizarDto → Duenio | DTO entrada → Entidad | Map(origen, destino) sobre la entidad existente | Ninguna |
| MascotaCrearDto → Mascota | DTO entrada → Entidad | Llega del cliente y se guarda | Ninguna |
| MascotaActualizarDto → Mascota | DTO entrada → Entidad | Map(origen, destino); no permite cambiar de dueño | Ninguna |

No se usa `ReverseMap()`: los DTOs de entrada y lectura son distintos, un mapeo inverso no tendría sentido.

[↑ Volver al índice](#indice)

---

<a id="seguridad"></a>
## 9. Seguridad: sin credenciales en el repositorio

- El repositorio no incluye contraseñas, tokens ni cadenas de conexión reales.
- La API lee la conexión con `GetConnectionString("VeterinariaDb")`. Cada persona define su propio valor localmente.
- Para configurarla sin guardarla en Git, usa User Secrets desde la carpeta del repositorio:
  ```bash
  dotnet user-secrets init --project Veterinaria.Api
  dotnet user-secrets set "ConnectionStrings:VeterinariaDb" "<tu-cadena-de-conexion>" --project Veterinaria.Api
  ```
- Alternativa: la variable de entorno `ConnectionStrings__VeterinariaDb`.
- La base de datos se crea con el script SQL incluido, no con datos privados.

[↑ Volver al índice](#indice)
