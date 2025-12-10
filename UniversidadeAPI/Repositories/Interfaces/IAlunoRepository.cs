using UniversidadeAPI.Entities;

namespace UniversidadeAPI.Repositories.Interfaces
{
    public interface IAlunoRepository : IGenericRepository<Aluno>
    {

        Task<Aluno?> GetByCPFAsync(string cpf);
    }
}