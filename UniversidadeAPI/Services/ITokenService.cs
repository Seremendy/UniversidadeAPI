using UniversidadeAPI.Entities;

namespace UniversidadeAPI.Services
{
    public interface ITokenService
    {
        string GenerateToken(Usuario usuarios);

    }
}
