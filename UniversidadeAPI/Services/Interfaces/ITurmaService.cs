using UniversidadeAPI.DTOs;

namespace UniversidadeAPI.Services.Interfaces
{
    public interface ITurmaService
    {
        Task<IEnumerable<TurmaResponseDto>> GetAllTurmasAsync();
        Task<TurmaResponseDto?> GetTurmaByIdAsync(int id);
        Task<TurmaResponseDto> CreateTurmaAsync(CreateTurmaRequestDto turmaDto);
        Task<bool> UpdateTurmaAsync(int id, UpdateTurmaRequestDto turmaDto);
        Task<bool> DeleteTurmaAsync(int id);
    }
}