using UniversidadeAPI.Entities;

namespace UniversidadeAPI.Repositories.Interfaces
{
    public interface ITurmaRepository : IGenericRepository<Turmas>
    {
        Task<IEnumerable<Turmas>> GetTurmasBySalaIdAsync(int salaDeAulaId);
    }
}
