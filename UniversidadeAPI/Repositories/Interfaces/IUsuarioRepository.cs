using UniversidadeAPI.Entities;

namespace UniversidadeAPI.Repositories.Interfaces
{

    public interface IUsuarioRepository
    {
        Task<Usuarios?> GetByLoginAsync(string login);
        Task<int> AddAsync(Usuarios usuario);
        Task<IEnumerable<Usuarios>> GetAllAsync();
        Task<bool> DeleteAsync(int id);
    }
}