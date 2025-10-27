using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using UniversidadeAPI.Models;
using UniversidadeAPI.Repositories; 
using UniversidadeAPI.Services;    

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
    public async Task<ActionResult> Login([FromBody] LoginRequestModel model)
    {
        var usuario = await _usuarioRepository.GetByLoginAsync(model.Login);

        if (usuario == null || !BCrypt.Net.BCrypt.Verify(model.SenhaHash, usuario.SenhaHash))
        {
            return Unauthorized(new { Message = "Login ou senha inválidos." });
        }

        var token = _tokenService.GenerateToken(usuario);

        // Retorna HTTP 200 OK com o token aninhado em um objeto JSON
        return Ok(new { token = token });
    }

    [HttpPost("register")]
    //[Authorize(Roles = "Admin")]
    public async Task<ActionResult> Register([FromBody] RegisterRequestModel model)
    {
        if (!Enum.TryParse(model.Role, true, out Role role))
        {
            return BadRequest(new { Message = "Role inválida. Use Admin, Professor ou Aluno." });
        }

        if (await _usuarioRepository.GetByLoginAsync(model.Login) != null)
        {
            return Conflict(new { Message = "Login já existe." });
        }

        var usuario = new Usuarios
        {
            Login = model.Login,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(model.SenhaHash),
            Role = role
        };

        var novoId = await _usuarioRepository.AddAsync(usuario);

        usuario.UsuarioID = novoId;

        return CreatedAtAction(null, new { id = usuario.UsuarioID }, usuario);
    }

}