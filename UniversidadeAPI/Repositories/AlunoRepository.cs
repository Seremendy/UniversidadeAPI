using System.Data;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories.Interfaces;
using Dapper;

namespace UniversidadeAPI.Repositories
{
    public class AlunoRepository : GenericRepository<Alunos>, IAlunoRepository
    {
        public AlunoRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }

        public async Task<Alunos?> GetByCPFAsync(string cpf)
        {
            var sql = $"SELECT * FROM {_tableName} WHERE CPF = @CPF";
            return await _dbConnection.QuerySingleOrDefaultAsync<Alunos>(sql, new { CPF = cpf });
        }
    }
}