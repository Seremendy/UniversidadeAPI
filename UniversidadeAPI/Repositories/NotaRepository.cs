using Dapper;
using System.Data;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories.Interfaces;

namespace UniversidadeAPI.Repositories
{
    public class NotaRepository : INotaRepository
    {
        private readonly IDbConnection _dbConnection;

        public NotaRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Notas>> GetAllAsync()
        {
            
            var sql = "SELECT * FROM notas";
            return await _dbConnection.QueryAsync<Notas>(sql);
        }

        public async Task<Notas?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM notas WHERE NotaID = @Id";
            return await _dbConnection.QuerySingleOrDefaultAsync<Notas>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(Notas entity)
        {
            
            var sql = @"
                INSERT INTO notas (NotaValor, AlunoID, DisciplinaID)
                VALUES (@NotaValor, @AlunoID, @DisciplinaID);
                SELECT LAST_INSERT_ID();";

            return await _dbConnection.ExecuteScalarAsync<int>(sql, new
            {
                entity.NotaValor, 
                entity.AlunoID,
                entity.DisciplinaID
            });
        }

        public async Task<bool> UpdateAsync(Notas entity)
        {
            
            var sql = @"
                UPDATE notas SET
                    NotaValor = @NotaValor,
                    AlunoID = @AlunoID,
                    DisciplinaID = @DisciplinaID
                WHERE NotaID = @NotaID";

            var rowsAffected = await _dbConnection.ExecuteAsync(sql, entity);
            
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "DELETE FROM notas WHERE NotaID = @Id";
            var rowsAffected = await _dbConnection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Notas>> GetNotasPorAlunoAsync(int alunoId)
        {
            
            var sql = "SELECT * FROM notas WHERE AlunoID = @AlunoId";
            return await _dbConnection.QueryAsync<Notas>(sql, new { AlunoId = alunoId });
        }

        public async Task<IEnumerable<Notas>> GetNotasPorDisciplinaAsync(int disciplinaId)
        {
            var sql = "SELECT * FROM notas WHERE DisciplinaID = @DisciplinaId";
            return await _dbConnection.QueryAsync<Notas>(sql, new { DisciplinaId = disciplinaId });
        }
    }
}