using Dapper;
using System.Data;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories.Interfaces;

namespace UniversidadeAPI.Repositories
{
    public class PrerequisitoRepository : IPrerequisitoRepository
    {
        private readonly IDbConnection _dbConnection;

        public PrerequisitoRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Prerequisitos>> GetAllAsync()
        {
            var sql = "SELECT * FROM Prerequisitos";
            return await _dbConnection.QueryAsync<Prerequisitos>(sql);
        }

        public async Task<IEnumerable<Prerequisitos>> GetPrerequisitosParaDisciplinaAsync(int disciplinaId)
        {
            var sql = "SELECT * FROM Prerequisitos WHERE DisciplinaID = @DisciplinaId";
            return await _dbConnection.QueryAsync<Prerequisitos>(sql, new { DisciplinaId = disciplinaId });
        }

        public async Task<int> AddAsync(Prerequisitos prerequisito)
        {
            var sql = @"
                INSERT INTO Prerequisitos (DisciplinaID, PreRequisitoID)
                VALUES (@DisciplinaID, @PreRequisitoID);";

            return await _dbConnection.ExecuteAsync(sql, prerequisito);
        }

        public async Task<bool> DeleteAsync(int disciplinaId, int preRequisitoId)
        {
            var sql = @"
                DELETE FROM Prerequisitos
                WHERE DisciplinaID = @DisciplinaID AND PreRequisitoID = @PreRequisitoID";

            var rowsAffected = await _dbConnection.ExecuteAsync(sql, new
            {
                DisciplinaID = disciplinaId,
                PreRequisitoID = preRequisitoId
            });

            return rowsAffected > 0;
        }
    }
}