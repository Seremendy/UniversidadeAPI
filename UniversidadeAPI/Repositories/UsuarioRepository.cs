using Dapper;
using System.Data;
using UniversidadeAPI.Models;

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

    }
}
