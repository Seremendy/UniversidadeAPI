using UniversidadeAPI.Entities;

namespace UniversidadeAPI.Repositories
{
    public interface INotaRepository : IGenericRepository<Nota>
    {
        Task<IEnumerable<Nota>> GetNotasPorAlunoAsync(int alunoId);

        Task<IEnumerable<Nota>> GetNotasPorDisciplinaAsync(int disciplinaId);
    }
}