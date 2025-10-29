using UniversidadeAPI.Models;

namespace UniversidadeAPI.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuarios?> GetByLoginAsync(string login);
        Task<int> AddAsync(Usuarios usuario);
    }
}
