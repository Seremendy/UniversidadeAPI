using System.Data;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories.Interfaces;

namespace UniversidadeAPI.Repositories
{
    public class ProfessorRepository : GenericRepository<Professor>, IProfessorRepository
    {
        public ProfessorRepository(IDbConnection dbConnection) : base(dbConnection)
        {
            // CORREÇÃO: O plural de Professor em português é "Professores"
            _tableName = "Professores";
        }
    }
}