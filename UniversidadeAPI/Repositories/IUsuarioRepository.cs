using UniversidadeAPI.Entities;

namespace UniversidadeAPI.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByLoginAsync(string login);
        Task<int> AddAsync(Usuario usuario);
    }
}
