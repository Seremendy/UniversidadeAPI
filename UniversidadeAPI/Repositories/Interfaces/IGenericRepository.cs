namespace UniversidadeAPI.Repositories.Interfaces
{
    // T será a sua classe de Modelo (Entidade), ex: Aluno, Curso, Professor
    public interface IGenericRepository<T> where T : class
    {
        // Contrato para buscar todos os itens
        Task<IEnumerable<T>> GetAllAsync();

        // Contrato para buscar um item pelo ID
        Task<T?> GetByIdAsync(int id);

        // Contrato para adicionar um novo item (retorna o novo ID)
        Task<int> AddAsync(T entity);

        // Contrato para atualizar um item (retorna true/false se foi bem-sucedido)
        Task<bool> UpdateAsync(T entity);

        // Contrato para apagar um item (retorna true/false se foi bem-sucedido)
        Task<bool> DeleteAsync(int id);
    }
}