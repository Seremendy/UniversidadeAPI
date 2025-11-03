using Dapper;
using System.Data;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories.Interfaces;

namespace UniversidadeAPI.Repositories
{
    public class CursoRepository : ICursoRepository
    {
        private readonly IDbConnection _dbConnection;

        public CursoRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Curso>> GetAllAsync()
        {
            var sql = "SELECT * FROM Cursos";
            return await _dbConnection.QueryAsync<Curso>(sql);
        }

        public async Task<Curso?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Cursos WHERE CursoID = @Id";
            return await _dbConnection.QuerySingleOrDefaultAsync<Curso>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(Curso entity)
        {
            var sql = @"
                INSERT INTO Cursos (NomeCurso, DepartamentoID)
                VALUES (@NomeCurso, @DepartamentoID);
                SELECT LAST_INSERT_ID();";

            return await _dbConnection.ExecuteScalarAsync<int>(sql, entity);
        }

        public async Task<bool> UpdateAsync(Curso entity)
        {
            var sql = @"
                UPDATE Cursos SET
                    NomeCurso = @NomeCurso,
                    DepartamentoID = @DepartamentoID
                WHERE CursoID = @CursoID";

            var rowsAffected = await _dbConnection.ExecuteAsync(sql, entity);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Cursos WHERE CursoID = @Id";
            var rowsAffected = await _dbConnection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}