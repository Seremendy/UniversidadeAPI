using UniversidadeAPI.Models;

namespace UniversidadeAPI.Repositories
{
    public interface IDepartamentoRepository
    {
        // CREATE: Adiciona um novo departamento e retorna seu ID
        Task<int> AddAsync(Departamento departamento);

        // READ (All): Retorna todos os departamentos
        Task<IEnumerable<Departamento>> GetAllAsync();

        // READ (ById): Retorna um departamento específico pelo ID
        Task<Departamento?> GetByIdAsync(int id);

        // UPDATE: Atualiza um departamento e retorna true se bem-sucedido
        Task<bool> UpdateAsync(Departamento departamento);

        // DELETE: Deleta um departamento pelo ID e retorna true se bem-sucedido
        Task<bool> DeleteAsync(int id);
    }
}