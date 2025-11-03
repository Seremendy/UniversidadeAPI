using Dapper;
using System.Data;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories.Interfaces;

namespace UniversidadeAPI.Repositories
{
    public class SalaDeAulaRepository : ISalaDeAulaRepository
    {
        private readonly IDbConnection _dbConnection;

        public SalaDeAulaRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<IEnumerable<SalaDeAula>> GetAllAsync()
        {
            var sql = "SELECT * FROM SalasDeAula";
            return await _dbConnection.QueryAsync<SalaDeAula>(sql);
        }

        public async Task<SalaDeAula?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM SalasDeAula WHERE SalaDeAulaID = @Id";
            return await _dbConnection.QuerySingleOrDefaultAsync<SalaDeAula>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(SalaDeAula entity)
        {
            var sql = @"
                INSERT INTO SalasDeAula (Capacidade, NumeroSala, PredioNome)
                VALUES (@Capacidade, @NumeroSala, @PredioNome);
                SELECT LAST_INSERT_ID();";

            return await _dbConnection.ExecuteScalarAsync<int>(sql, entity);
        }

        public async Task<bool> UpdateAsync(SalaDeAula entity)
        {
            var sql = @"
                UPDATE SalasDeAula SET
                    Capacidade = @Capacidade,
                    NumeroSala = @NumeroSala,
                    PredioNome = @PredioNome
                WHERE SalaDeAulaID = @SalaDeAulaID";

            var rowsAffected = await _dbConnection.ExecuteAsync(sql, entity);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "DELETE FROM SalasDeAula WHERE SalaDeAulaID = @Id";
            var rowsAffected = await _dbConnection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}