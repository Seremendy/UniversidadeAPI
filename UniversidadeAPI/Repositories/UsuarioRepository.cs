using Dapper;
using System.Data;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories.Interfaces;

namespace UniversidadeAPI.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IDbConnection _dbConnection;
        public UsuarioRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Usuarios?> GetByLoginAsync(string login)
        {
            var sql = "SELECT UsuarioID, Login, SenhaHash, Role FROM Usuarios WHERE Login = @Login";
            return await _dbConnection.QuerySingleOrDefaultAsync<Usuarios>(sql, new { Login = login });
        }

        public async Task<int> AddAsync(Usuarios usuario)
        {
            var sql = @"
                INSERT INTO Usuarios (Login, SenhaHash, Role)
                VALUES (@Login, @SenhaHash, @Role);
                SELECT LAST_INSERT_ID();";

            return await _dbConnection.ExecuteScalarAsync<int>(sql, new
            {
                usuario.Login,
                usuario.SenhaHash,
                Role = usuario.Role.ToString()
            });
        }

        // --- Implementação dos novos métodos ---

        public async Task<IEnumerable<Usuarios>> GetAllAsync()
        {
            // Selecionamos apenas os dados seguros (sem a senha)
            var sql = "SELECT UsuarioID, Login, Role FROM Usuarios";
            return await _dbConnection.QueryAsync<Usuarios>(sql);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Usuarios WHERE UsuarioID = @Id";
            var rowsAffected = await _dbConnection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}