using UniversidadeAPI.Entities;

namespace UniversidadeAPI.Repositories.Interfaces
{
    public interface IAlunoRepository : IGenericRepository<Alunos>
    {

        Task<Alunos?> GetByCPFAsync(string cpf);
    }
}