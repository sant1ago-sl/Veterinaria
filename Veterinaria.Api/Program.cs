using Microsoft.EntityFrameworkCore;
using Veterinaria.Datos;
using Veterinaria.Datos.Repositorios;
using Veterinaria.Negocio.Mapeos;
using Veterinaria.Negocio.Servicios;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<VeterinariaContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("VeterinariaDb")));

builder.Services.AddScoped<IDuenioRepository, DuenioRepository>();
builder.Services.AddScoped<IMascotaRepository, MascotaRepository>();
builder.Services.AddScoped<IDuenioService, DuenioService>();
builder.Services.AddScoped<IMascotaService, MascotaService>();

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
