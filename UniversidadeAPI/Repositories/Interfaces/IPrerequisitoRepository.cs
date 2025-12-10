using UniversidadeAPI.Entities;

namespace UniversidadeAPI.Repositories.Interfaces
{
    // Esta interface NÃO usa IGenericRepository
    public interface IPrerequisitoRepository
    {
        
        Task<IEnumerable<Prerequisito>> GetAllAsync();

        
        Task<IEnumerable<Prerequisito>> GetPrerequisitosParaDisciplinaAsync(int disciplinaId);

        
        Task<int> AddAsync(Prerequisito prerequisito);

        
        Task<bool> DeleteAsync(int disciplinaId, int preRequisitoId);
    }
}