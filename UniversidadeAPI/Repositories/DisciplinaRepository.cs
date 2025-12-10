using System.Data;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories.Interfaces;

namespace UniversidadeAPI.Repositories
{
    public class DisciplinaRepository : GenericRepository<Disciplina>, IDisciplinaRepository
    {
        public DisciplinaRepository(IDbConnection dbConnection) : base(dbConnection)
        {
           
        }
    }
}