using Dapper;
using System.Data;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories.Interfaces;

namespace UniversidadeAPI.Repositories
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }

        public async Task<Usuario?> GetByLoginAsync(string login)
        {
            var sql = $"SELECT * FROM {_tableName} WHERE Login = @Login";
            return await _dbConnection.QuerySingleOrDefaultAsync<Usuario>(sql, new { Login = login });
        }

        public override async Task<int> AddAsync(Usuario usuario)
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