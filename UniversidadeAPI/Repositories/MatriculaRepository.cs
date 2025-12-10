using System.Data;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories.Interfaces;

namespace UniversidadeAPI.Repositories
{
    public class MatriculaRepository : GenericRepository<Matricula>, IMatriculaRepository
    {
        public MatriculaRepository(IDbConnection dbConnection) : base(dbConnection)
        {
            _tableName = "Matricula";
        }
    }
}