using Dapper;
using System.Data;
using UniversidadeAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace UniversidadeAPI.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly IConfiguration _configuration;
        public UsuarioRepository(IDbConnection dbConnection, IConfiguration configuration)
        {
            _dbConnection = dbConnection;
            _configuration = configuration;
        }

        public async Task<Usuarios?> GetByLoginAsync(string login)
        {
            var sql = "SELECT UsuarioID, Login, SenhaHash, Role FROM Usuarios WHERE Login = @Login";
            return await _dbConnection.QuerySingleOrDefaultAsync<Usuarios>(sql, new { Login = login });
        }

        public Task<Usuarios> GetByLoginAsync(object login)
        {
            throw new NotImplementedException();
        }

        public Task<Usuarios?> GetByPasswordAsync(string password)
        {
            throw new NotImplementedException();
        }

        public Task<Usuarios?> GetByRoleAsync(Enum role)
        {
            throw new NotImplementedException();
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

        public string GenerateJwtToken(Usuarios usuario)
        {
            var now = DateTime.UtcNow;
            var expires = now.AddHours(1);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", usuario.UsuarioID.ToString()) }),
                NotBefore = now,
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
