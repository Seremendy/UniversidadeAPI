using UniversidadeAPI.Entities;

namespace UniversidadeAPI.Repositories.Interfaces
{
    public interface ITurmaRepository : IGenericRepository<Turma>
    {
        Task<IEnumerable<Turma>> GetTurmasBySalaIdAsync(int salaDeAulaId);
    }
}
