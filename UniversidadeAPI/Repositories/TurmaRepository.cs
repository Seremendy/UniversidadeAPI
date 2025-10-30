﻿using Dapper;
using System.Data;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories;

namespace UniversidadeAPI.Repositories
{
    public class TurmaRepository : ITurmaRepository
    {
        private readonly IDbConnection _dbConnection;

        public TurmaRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Turma>> GetAllAsync()
        {
            var sql = "SELECT * FROM Turmas";
            return await _dbConnection.QueryAsync<Turma>(sql);
        }

        public async Task<Turma?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Turmas WHERE TurmaID = @Id";
            return await _dbConnection.QuerySingleOrDefaultAsync<Turma>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(Turma entity)
        {
            var sql = @"
                INSERT INTO Turmas (Semestre, DisciplinaID, SalaDeAulaID, ProfessorID, HorarioID)
                VALUES (@Semestre, @DisciplinaID, @SalaDeAulaID, @ProfessorID, @HorarioID);
                SELECT LAST_INSERT_ID();";

            return await _dbConnection.ExecuteScalarAsync<int>(sql, entity);
        }

        public async Task<bool> UpdateAsync(Turma entity)
        {
            var sql = @"
                UPDATE Turmas SET
                    Semestre = @Semestre,
                    DisciplinaID = @DisciplinaID,
                    SalaDeAulaID = @SalaDeAulaID,
                    ProfessorID = @ProfessorID,
                    HorarioID = @HorarioID
                WHERE TurmaID = @TurmaID";

            var rowsAffected = await _dbConnection.ExecuteAsync(sql, entity);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Turmas WHERE TurmaID = @Id";
            var rowsAffected = await _dbConnection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}