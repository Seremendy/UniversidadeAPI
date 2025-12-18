using System.Data;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories.Interfaces;

namespace UniversidadeAPI.Repositories
{
    public class ProfessorRepository : GenericRepository<Professores>, IProfessorRepository
    {
        public ProfessorRepository(IDbConnection dbConnection) : base(dbConnection)
        {
            
            _tableName = "Professores";
        }
    }
}