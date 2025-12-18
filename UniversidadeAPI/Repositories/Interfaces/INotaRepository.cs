using UniversidadeAPI.Entities;

namespace UniversidadeAPI.Repositories.Interfaces
{
    public interface INotaRepository : IGenericRepository<Notas>
    {
        Task<IEnumerable<Notas>> GetNotasPorAlunoAsync(int alunoId);
    }
}