using Dapper;
using System.Data;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories.Interfaces;

namespace UniversidadeAPI.Repositories
{
    public class HorarioRepository : IHorarioRepository
    {
        private readonly IDbConnection _dbConnection;
        public HorarioRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Horarios>> GetAllAsync()
        {
            var sql = "SELECT * FROM Horarios";
            return await _dbConnection.QueryAsync<Horarios>(sql);
        }

        public async Task<Horarios?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Horarios WHERE HorarioID = @Id";
            return await _dbConnection.QuerySingleOrDefaultAsync<Horarios>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(Horarios entity)
        {
            var sql = @"
                INSERT INTO Horarios (DiaSemana, HoraInicio, HoraFim)
                VALUES (@DiaSemana, @HoraInicio, @HoraFim);
                SELECT LAST_INSERT_ID();";

            return await _dbConnection.ExecuteScalarAsync<int>(sql, new
            {
                DiaSemana = entity.DiaSemana.ToString(),
                entity.HoraInicio,
                entity.HoraFim
            });
        }

        public async Task<bool> UpdateAsync(Horarios entity)
        {
            var sql = @"
                UPDATE Horarios SET
                    DiaSemana = @DiaSemana,
                    HoraInicio = @HoraInicio,
                    HoraFim = @HoraFim
                WHERE HorarioID = @HorarioID";

            var rowsAffected = await _dbConnection.ExecuteAsync(sql, new
            {
                DiaSemana = entity.DiaSemana.ToString(),
                entity.HoraInicio,
                entity.HoraFim,
                entity.HorarioID
            });
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Horarios WHERE HorarioID = @Id";
            var rowsAffected = await _dbConnection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}