using Dapper;
using System.Data;
using UniversidadeAPI.Entities; // Namespace das suas Entidades/Modelos
using UniversidadeAPI.Repositories;

namespace UniversidadeAPI.Repositories
{
    public class ProfessorRepository : IProfessorRepository
    {
        private readonly IDbConnection _dbConnection;

        public ProfessorRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Professor>> GetAllAsync()
        {
            var sql = "SELECT * FROM Professores";
            return await _dbConnection.QueryAsync<Professor>(sql);
        }

        public async Task<Professor?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Professores WHERE ProfessorID = @Id";
            return await _dbConnection.QuerySingleOrDefaultAsync<Professor>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(Professor entity)
        {
            var sql = @"
                INSERT INTO Professores (ProfessorNome, DataNascimento, RG, CPF, Formacao)
                VALUES (@ProfessorNome, @DataNascimento, @RG, @CPF, @Formacao);
                SELECT LAST_INSERT_ID();";

            return await _dbConnection.ExecuteScalarAsync<int>(sql, entity);
        }

        public async Task<bool> UpdateAsync(Professor entity)
        {
            var sql = @"
                UPDATE Professores SET
                    ProfessorNome = @ProfessorNome,
                    DataNascimento = @DataNascimento,
                    RG = @RG,
                    CPF = @CPF,
                    Formacao = @Formacao
                WHERE ProfessorID = @ProfessorID";

            var rowsAffected = await _dbConnection.ExecuteAsync(sql, entity);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Professores WHERE ProfessorID = @Id";
            var rowsAffected = await _dbConnection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

    }
}