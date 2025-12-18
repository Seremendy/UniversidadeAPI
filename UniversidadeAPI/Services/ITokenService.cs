using UniversidadeAPI.Entities;

namespace UniversidadeAPI.Services
{
    public interface ITokenService
    {
        string GenerateToken(Usuarios usuarios);

    }
}
