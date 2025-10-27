using UniversidadeAPI.Models;

namespace UniversidadeAPI.Services
{
    public interface ITokenService
    {
        string GenerateToken(Usuarios usuarios);

    }
}
