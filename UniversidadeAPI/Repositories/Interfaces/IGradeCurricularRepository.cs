using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories.Interfaces;
namespace UniversidadeAPI.Repositories
{

    public interface IGradeCurricularRepository : IGenericRepository<GradeCurricular>
    {
        Task<IEnumerable<GradeCurricular>> GetGradeByCursoIdAsync(int cursoId);
    }
}