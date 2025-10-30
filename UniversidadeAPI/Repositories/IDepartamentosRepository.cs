using UniversidadeAPI.Entities;

namespace UniversidadeAPI.Repositories
{
    public interface IDepartamentoRepository
    {

        Task<int> AddAsync(Departamento departamento);

        Task<IEnumerable<Departamento>> GetAllAsync();

        Task<Departamento?> GetByIdAsync(int id);

        Task<bool> UpdateAsync(Departamento departamento);

        Task<bool> DeleteAsync(int id);
    }
}