using Dapper;
using System.Data;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories.Interfaces;

namespace UniversidadeAPI.Repositories
{
    public class AlunoRepository : IAlunoRepository
    {
        private readonly IDbConnection _dbConnection;

        public AlunoRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Aluno>> GetAllAsync()
        {
            var sql = "SELECT * FROM Alunos";
            return await _dbConnection.QueryAsync<Aluno>(sql);
        }

        public async Task<Aluno?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Alunos WHERE AlunoID = @Id";
            return await _dbConnection.QuerySingleOrDefaultAsync<Aluno>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(Aluno entity)
        {
            var sql = @"
                INSERT INTO Alunos (AlunoNome, DataNascimento, RG, CPF)
                VALUES (@AlunoNome, @DataNascimento, @RG, @CPF);
                SELECT LAST_INSERT_ID();";

            return await _dbConnection.ExecuteScalarAsync<int>(sql, entity);
        }

        public async Task<bool> UpdateAsync(Aluno entity)
        {
            var sql = @"
                UPDATE Alunos SET
                    AlunoNome = @AlunoNome,
                    DataNascimento = @DataNascimento,
                    RG = @RG,
                    CPF = @CPF
                WHERE AlunoID = @AlunoID";

            var rowsAffected = await _dbConnection.ExecuteAsync(sql, entity);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Alunos WHERE AlunoID = @Id";
            var rowsAffected = await _dbConnection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        // --- Implementação do método específico ---
        public async Task<Aluno?> GetByCPFAsync(string cpf)
        {
            var sql = "SELECT * FROM Alunos WHERE CPF = @CPF";
            return await _dbConnection.QuerySingleOrDefaultAsync<Aluno>(sql, new { CPF = cpf });
        }
    }
}