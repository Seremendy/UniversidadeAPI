using UniversidadeAPI.Entities;

namespace UniversidadeAPI.Repositories.Interfaces
{
    public interface INotaRepository : IGenericRepository<Nota>
    {
        Task<IEnumerable<Nota>> GetNotasPorAlunoAsync(int alunoId);

        Task<IEnumerable<Nota>> GetNotasPorDisciplinaAsync(int disciplinaId);
    }
}