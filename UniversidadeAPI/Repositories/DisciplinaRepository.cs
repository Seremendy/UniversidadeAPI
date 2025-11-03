using Dapper;
using System.Data;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories.Interfaces;

namespace UniversidadeAPI.Repositories
{
    public class DisciplinaRepository : IDisciplinaRepository
    {
        private readonly IDbConnection _dbConnection;

        public DisciplinaRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }


        public async Task<IEnumerable<Disciplina>> GetAllAsync()
        {
            var sql = "SELECT * FROM Disciplinas";
            return await _dbConnection.QueryAsync<Disciplina>(sql);
        }

        public async Task<Disciplina?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Disciplinas WHERE DisciplinaID = @Id";
            return await _dbConnection.QuerySingleOrDefaultAsync<Disciplina>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(Disciplina entity)
        {
            var sql = @"
                INSERT INTO Disciplinas (NomeDisciplina)
                VALUES (@NomeDisciplina);
                SELECT LAST_INSERT_ID();";

            return await _dbConnection.ExecuteScalarAsync<int>(sql, entity);
        }

        public async Task<bool> UpdateAsync(Disciplina entity)
        {
            var sql = @"
                UPDATE Disciplinas SET
                    NomeDisciplina = @NomeDisciplina
                WHERE DisciplinaID = @DisciplinaID";

            var rowsAffected = await _dbConnection.ExecuteAsync(sql, entity);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Disciplinas WHERE DisciplinaID = @Id";
            var rowsAffected = await _dbConnection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}