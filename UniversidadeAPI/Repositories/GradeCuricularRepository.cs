using Dapper;
using System.Data;
using UniversidadeAPI.Entities;

namespace UniversidadeAPI.Repositories
{
    public class GradeCurricularRepository : IGradeCurricularRepository
    {
        private readonly IDbConnection _dbConnection;

        public GradeCurricularRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<GradeCurricular>> GetAllAsync()
        {
            var sql = "SELECT * FROM GradeCurriculares";
            return await _dbConnection.QueryAsync<GradeCurricular>(sql);
        }

        public async Task<GradeCurricular?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM GradeCurriculares WHERE GradeCurricularID = @Id";
            return await _dbConnection.QuerySingleOrDefaultAsync<GradeCurricular>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(GradeCurricular entity)
        {
            var sql = @"
                INSERT INTO GradeCurriculares (DisciplinaID, CursoID)
                VALUES (@DisciplinaID, @CursoID);
                SELECT LAST_INSERT_ID();";

            return await _dbConnection.ExecuteScalarAsync<int>(sql, entity);
        }

        public async Task<bool> UpdateAsync(GradeCurricular entity)
        {
            var sql = @"
                UPDATE GradeCurriculares SET
                    DisciplinaID = @DisciplinaID,
                    CursoID = @CursoID
                WHERE GradeCurricularID = @GradeCurricularID";

            var rowsAffected = await _dbConnection.ExecuteAsync(sql, entity);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "DELETE FROM GradeCurriculares WHERE GradeCurricularID = @Id";
            var rowsAffected = await _dbConnection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }


        public async Task<IEnumerable<GradeCurricular>> GetGradeByCursoIdAsync(int cursoId)
        {
            var sql = "SELECT * FROM GradeCurriculares WHERE CursoID = @CursoId";
            return await _dbConnection.QueryAsync<GradeCurricular>(sql, new { CursoId = cursoId });
        }
    }
}