using UniversidadeAPI.Entities;

namespace UniversidadeAPI.Repositories
{
    public interface IAlunoRepository : IGenericRepository<Aluno>
    {
        Task<Aluno?> GetByCPFAsync(string cpf);
    }
}
