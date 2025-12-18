using UniversidadeAPI.Entities;

namespace UniversidadeAPI.Repositories.Interfaces
{
    public interface IDepartamentoRepository
    {

        Task<int> AddAsync(Departamentos departamento);

        Task<IEnumerable<Departamentos>> GetAllAsync();

        Task<Departamentos?> GetByIdAsync(int id);

        Task<bool> UpdateAsync(Departamentos departamento);

        Task<bool> DeleteAsync(int id);
    }
}