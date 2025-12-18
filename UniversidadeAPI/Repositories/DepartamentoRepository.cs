using Dapper; 
using MySqlConnector; 
using System.Data;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories.Interfaces;

namespace UniversidadeAPI.Repositories
{
    public class DepartamentoRepository : IDepartamentoRepository
    {
        private readonly IDbConnection _dbConnection;

        public DepartamentoRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Departamentos?> GetByIdAsync(int id)
        {
            var sql = "SELECT DepartamentoID, DepartamentoNome FROM Departamentos WHERE DepartamentoID = @Id";
            return await _dbConnection.QuerySingleOrDefaultAsync<Departamentos>(sql, new { Id = id });

        }

        public async Task<int> AddAsync(Departamentos departamento)
        { 
            var sql = "INSERT INTO Departamentos (DepartamentoNome) VALUES (@DepartamentoNome); SELECT LAST_INSERT_ID();";
            return await _dbConnection.ExecuteScalarAsync<int>(sql, departamento);
        }

        public async Task<IEnumerable<Departamentos>> GetAllAsync()
        {
            var sql = "SELECT DepartamentoID, DepartamentoNome FROM Departamentos";
            return await _dbConnection.QueryAsync<Departamentos>(sql);
        }

        public async Task<bool> UpdateAsync(Departamentos departamento)
        {
            var sql = "UPDATE Departamentos SET DepartamentoNome = @DepartamentoNome WHERE DepartamentoID = @DepartamentoID";
            var rowsAffected = await _dbConnection.ExecuteAsync(sql, departamento);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Departamentos WHERE DepartamentoID = @Id";
            var rowsAffected = await _dbConnection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

    }
}