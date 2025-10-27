using UniversidadeAPI.Models;

namespace UniversidadeAPI.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuarios?> GetByLoginAsync(string login);
        Task<Usuarios> GetByLoginAsync(object login);
        Task<Usuarios?> GetByPasswordAsync(string password);

        Task<Usuarios?> GetByRoleAsync(Enum role);

        Task<int> AddAsync(Usuarios usuario);

    }
}
