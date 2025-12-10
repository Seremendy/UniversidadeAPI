using UniversidadeAPI.Entities;

namespace UniversidadeAPI.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByLoginAsync(string login);
        Task<int> AddAsync(Usuario usuario);
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<bool> DeleteAsync(int id);
    }
}
