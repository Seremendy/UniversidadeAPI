using Dapper;
using System.Data;
using System.Linq;
using System.Reflection;
using UniversidadeAPI.Repositories.Interfaces;

namespace UniversidadeAPI.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly IDbConnection _dbConnection;
        protected string _tableName;
        protected string _primaryKey;

        public GenericRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;

            _tableName = typeof(T).Name;

            _primaryKey = typeof(T).Name + "ID";
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var sql = $"SELECT * FROM {_tableName}";
            return await _dbConnection.QueryAsync<T>(sql);
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            var sql = $"SELECT * FROM {_tableName} WHERE {_primaryKey} = @Id";
            return await _dbConnection.QuerySingleOrDefaultAsync<T>(sql, new { Id = id });
        }

        public virtual async Task<int> AddAsync(T entity)
        {
            var properties = GetProperties(entity);
            var columns = string.Join(", ", properties);
            var parameters = string.Join(", ", properties.Select(p => "@" + p));

            var sql = $"INSERT INTO {_tableName} ({columns}) VALUES ({parameters}); SELECT LAST_INSERT_ID();";

            return await _dbConnection.ExecuteScalarAsync<int>(sql, entity);
        }

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            var properties = GetProperties(entity);
            var updateClause = string.Join(", ", properties.Select(p => $"{p} = @{p}"));

            var sql = $"UPDATE {_tableName} SET {updateClause} WHERE {_primaryKey} = @{_primaryKey}";

            var rowsAffected = await _dbConnection.ExecuteAsync(sql, entity);
            return rowsAffected > 0;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var sql = $"DELETE FROM {_tableName} WHERE {_primaryKey} = @Id";
            var rowsAffected = await _dbConnection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        private IEnumerable<string> GetProperties(T entity)
        {
            return typeof(T).GetProperties()
                .Where(p => p.Name != _primaryKey &&
                           (p.PropertyType.IsValueType || p.PropertyType == typeof(string)))
                .Select(p => p.Name);
        }
    }
}