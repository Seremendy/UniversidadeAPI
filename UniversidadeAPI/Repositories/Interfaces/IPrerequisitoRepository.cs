using UniversidadeAPI.Entities;

namespace UniversidadeAPI.Repositories.Interfaces
{
    // Esta interface NÃO usa IGenericRepository
    public interface IPrerequisitoRepository
    {
        
        Task<IEnumerable<Prerequisitos>> GetAllAsync();

        
        Task<IEnumerable<Prerequisitos>> GetPrerequisitosParaDisciplinaAsync(int disciplinaId);

        
        Task<int> AddAsync(Prerequisitos prerequisito);

        
        Task<bool> DeleteAsync(int disciplinaId, int preRequisitoId);
    }
}