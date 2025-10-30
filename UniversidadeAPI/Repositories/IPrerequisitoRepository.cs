using UniversidadeAPI.Entities;

namespace UniversidadeAPI.Repositories
{
    // Esta interface NÃO usa IGenericRepository
    public interface IPrerequisitoRepository
    {
        // Retorna todos os pré-requisitos (ex: [SQL -> Cálculo I], [Física I -> Física II])
        Task<IEnumerable<Prerequisito>> GetAllAsync();

        // Retorna todos os pré-requisitos para UMA disciplina
        // (Ex: Para "Cálculo II", retorna "Cálculo I" e "Álgebra Linear")
        Task<IEnumerable<Prerequisito>> GetPrerequisitosParaDisciplinaAsync(int disciplinaId);

        // Adiciona uma nova relação de pré-requisito
        Task<int> AddAsync(Prerequisito prerequisito);

        // Remove uma relação de pré-requisito (usa a chave composta)
        Task<bool> DeleteAsync(int disciplinaId, int preRequisitoId);
    }
}