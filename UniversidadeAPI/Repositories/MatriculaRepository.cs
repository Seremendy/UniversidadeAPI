using Dapper;
using System.Data;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories.Interfaces;

namespace UniversidadeAPI.Repositories
{
    public class MatriculaRepository : IMatriculaRepository
    {
        private readonly IDbConnection _dbConnection;

        public MatriculaRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }


        public async Task<IEnumerable<Matricula>> GetAllAsync()
        {
            var sql = "SELECT * FROM Matriculas";
            return await _dbConnection.QueryAsync<Matricula>(sql);
        }

        public async Task<Matricula?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Matriculas WHERE MatriculaID = @Id";
            return await _dbConnection.QuerySingleOrDefaultAsync<Matricula>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(Matricula entity)
        {
            var sql = @"
                INSERT INTO Matriculas (AlunoID, CursoID, DataMatricula, MatriculaAtiva)
                VALUES (@AlunoID, @CursoID, @DataMatricula, @MatriculaAtiva);
                SELECT LAST_INSERT_ID();";

            return await _dbConnection.ExecuteScalarAsync<int>(sql, entity);
        }

        public async Task<bool> UpdateAsync(Matricula entity)
        {
            var sql = @"
                UPDATE Matriculas SET
                    AlunoID = @AlunoID,
                    CursoID = @CursoID,
                    DataMatricula = @DataMatricula,
                    MatriculaAtiva = @MatriculaAtiva
                WHERE MatriculaID = @MatriculaID";

            var rowsAffected = await _dbConnection.ExecuteAsync(sql, entity);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Matriculas WHERE MatriculaID = @Id";
            var rowsAffected = await _dbConnection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}