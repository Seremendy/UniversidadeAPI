using Dapper;
using System.Data;
using UniversidadeAPI.Entities;


namespace UniversidadeAPI.Repositories
{
    public class NotaRepository : INotaRepository
    {
        private readonly IDbConnection _dbConnection;

        public NotaRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Nota>> GetAllAsync()
        {
            var sql = "SELECT * FROM Notas";
            return await _dbConnection.QueryAsync<Nota>(sql);
        }

        public async Task<Nota?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Notas WHERE NotaID = @Id";
            return await _dbConnection.QuerySingleOrDefaultAsync<Nota>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(Nota entity)
        {
            var sql = @"
                INSERT INTO Notas (Nota, AlunoID, DisciplinaID)
                VALUES (@NotaValor, @AlunoID, @DisciplinaID);
                SELECT LAST_INSERT_ID();";

            return await _dbConnection.ExecuteScalarAsync<int>(sql, new
            {
                entity.NotaValor,
                entity.AlunoID,
                entity.DisciplinaID
            });
        }

        public async Task<bool> UpdateAsync(Nota entity)
        {
            var sql = @"
                UPDATE Notas SET
                    Nota = @NotaValor,
                    AlunoID = @AlunoID,
                    DisciplinaID = @DisciplinaID
                WHERE NotaID = @NotaID";

            var rowsAffected = await _dbConnection.ExecuteAsync(sql, new
            {
                entity.NotaValor,
                entity.AlunoID,
                entity.DisciplinaID,
                entity.NotaID
            });
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Notas WHERE NotaID = @Id";
            var rowsAffected = await _dbConnection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        // --- Implementação dos métodos específicos ---

        public async Task<IEnumerable<Nota>> GetNotasPorAlunoAsync(int alunoId)
        {
            var sql = "SELECT * FROM Notas WHERE AlunoID = @AlunoId";
            return await _dbConnection.QueryAsync<Nota>(sql, new { AlunoId = alunoId });
        }

        public async Task<IEnumerable<Nota>> GetNotasPorDisciplinaAsync(int disciplinaId)
        {
            var sql = "SELECT * FROM Notas WHERE DisciplinaID = @DisciplinaId";
            return await _dbConnection.QueryAsync<Nota>(sql, new { DisciplinaId = disciplinaId });
        }
    }
}