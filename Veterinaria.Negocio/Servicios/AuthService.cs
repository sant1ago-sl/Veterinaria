using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Veterinaria.Datos.Repositorios;
using Veterinaria.Entidades.Dtos;
using Veterinaria.Entidades.Models;
using Veterinaria.Negocio.Excepciones;

namespace Veterinaria.Negocio.Servicios;

public class AuthService : IAuthService
{
    public const string RolRecepcionista = "Recepcionista";
    public const string RolVeterinario = "Veterinario";

    private readonly IUsuarioRepository _repo;
    private readonly JwtSettings _jwt;

    public AuthService(IUsuarioRepository repo, JwtSettings jwt)
    {
        _repo = repo;
        _jwt = jwt;
    }

    public async Task<UsuarioDto> RegistrarAsync(RegistroDto dto)
    {
        var correo = dto.Correo.Trim().ToLowerInvariant();

        if (await _repo.ObtenerPorCorreoAsync(correo) is not null)
            throw new ReglaNegocioException($"Ya existe un usuario con el correo '{correo}'.");

        var usuario = new Usuario
        {
            Correo = correo,
            ClaveHash = BCrypt.Net.BCrypt.HashPassword(dto.Clave),
            Rol = RolRecepcionista
        };

        await _repo.AgregarAsync(usuario);
        await _repo.GuardarAsync();

        return new UsuarioDto { Id = usuario.Id, Correo = usuario.Correo, Rol = usuario.Rol };
    }

    public async Task<TokenDto?> LoginAsync(LoginDto dto)
    {
        var correo = dto.Correo.Trim().ToLowerInvariant();
        var usuario = await _repo.ObtenerPorCorreoAsync(correo);

        if (usuario is null || !BCrypt.Net.BCrypt.Verify(dto.Clave, usuario.ClaveHash))
            return null;

        return GenerarToken(usuario);
    }

    private TokenDto GenerarToken(Usuario usuario)
    {
        var expiracion = DateTime.UtcNow.AddMinutes(_jwt.ExpireMinutes);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Correo),
            new Claim("role", usuario.Rol),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiracion,
            signingCredentials: credenciales);

        return new TokenDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiracion = expiracion
        };
    }
}
