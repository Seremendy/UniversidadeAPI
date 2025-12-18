using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories.Interfaces;
namespace UniversidadeAPI.Repositories
{

    public interface IGradeCurricularRepository : IGenericRepository<GradeCurriculares>
    {
        Task<IEnumerable<GradeCurriculares>> GetGradeByCursoIdAsync(int cursoId);
    }
}