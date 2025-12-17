using UniversidadeAPI.Entities;

namespace UniversidadeAPI.Repositories.Interfaces
{
    // Removemos ": IGenericRepository<Usuario>" para ter controle total sobre os métodos
    // e evitar a obrigatoriedade de implementar Update/GetById neste momento.
    public interface IUsuarioRepository
    {
        // Método específico de Login
        Task<Usuario?> GetByLoginAsync(string login);

        // Métodos que replicam o comportamento necessário do CRUD
        Task<int> AddAsync(Usuario usuario);
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<bool> DeleteAsync(int id);
    }
}