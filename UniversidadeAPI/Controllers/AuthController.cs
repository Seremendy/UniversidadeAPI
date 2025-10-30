using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories;
using UniversidadeAPI.Services;
using UniversidadeAPI.DTOs.Auth;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITokenService _tokenService;

    public AuthController(IUsuarioRepository usuarioRepository, ITokenService tokenService)
    {
        _usuarioRepository = usuarioRepository;
        _tokenService = tokenService;
    }


    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var usuario = await _usuarioRepository.GetByLoginAsync(dto.Login);

        if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
        {
            return Unauthorized(new { Message = "Login ou senha inválidos." });
        }

        var token = _tokenService.GenerateToken(usuario);

        return Ok(new { token = token });
    }

    [HttpPost("register")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Register([FromBody] RegisterRequestDto dto)
    {
        if (!Enum.TryParse(dto.Role, true, out Role role))
        {
            return BadRequest(new { Message = "Role inválida. Use Admin, Professor ou Aluno." });
        }

        if (await _usuarioRepository.GetByLoginAsync(dto.Login) != null)
        {
            return Conflict(new { Message = "Login já existe." });
        }

       
        var usuario = new Usuario
        {
            Login = dto.Login,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
            Role = role
        };
        

        var novoId = await _usuarioRepository.AddAsync(usuario);
        usuario.UsuarioID = novoId;

     
        var responseDto = new UsuarioResponseDto
        {
            UsuarioID = usuario.UsuarioID,
            Login = usuario.Login,
            Role = usuario.Role.ToString()
        };

        return CreatedAtAction(null, new { id = responseDto.UsuarioID }, responseDto);
    }
}