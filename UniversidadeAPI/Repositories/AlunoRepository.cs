using System.Data;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories.Interfaces;
using Dapper;

namespace UniversidadeAPI.Repositories
{
    public class AlunoRepository : GenericRepository<Aluno>, IAlunoRepository
    {
        public AlunoRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }

        public async Task<Aluno?> GetByCPFAsync(string cpf)
        {
            var sql = $"SELECT * FROM {_tableName} WHERE CPF = @CPF";
            return await _dbConnection.QuerySingleOrDefaultAsync<Aluno>(sql, new { CPF = cpf });
        }
    }
}