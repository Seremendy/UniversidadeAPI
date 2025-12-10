using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Services;
using UniversidadeAPI.DTOs.Auth;
using UniversidadeAPI.Repositories.Interfaces;

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
    [AllowAnonymous]
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

    [HttpGet("users")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UsuarioResponseDto>>> GetUsers()
    {
        // Lembre-se de implementar o GetAllAsync no Repositório antes!
        var usuarios = await _usuarioRepository.GetAllAsync();

        var response = usuarios.Select(u => new UsuarioResponseDto
        {
            UsuarioID = u.UsuarioID,
            Login = u.Login,
            Role = u.Role.ToString()
        });

        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // Segurança máxima: Só Admin deleta!
    public async Task<ActionResult> DeleteUser(int id)
    {
        // Opcional: Impedir que o Admin se auto-delete (evita trancar o sistema fora)
        // Você precisaria pegar o ID do token, mas por enquanto vamos deixar simples.

        var sucesso = await _usuarioRepository.DeleteAsync(id);

        if (!sucesso) return NotFound(new { Message = "Usuário não encontrado." });

        return NoContent(); 
    }
}