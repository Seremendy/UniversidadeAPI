using AutoMapper; // <--- Importante
using UniversidadeAPI.DTOs;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories.Interfaces;
using UniversidadeAPI.Services.Interfaces;

namespace UniversidadeAPI.Services
{
    public class TurmaService : ITurmaService
    {
        private readonly ITurmaRepository _turmaRepository;
        private readonly IDisciplinaRepository _disciplinaRepository;
        private readonly ISalaDeAulaRepository _salaDeAulaRepository;
        private readonly IProfessorRepository _professorRepository;
        private readonly IHorarioRepository _horarioRepository;
        private readonly IMapper _mapper; // <--- Injetamos o AutoMapper

        public TurmaService(
            ITurmaRepository turmaRepository,
            IDisciplinaRepository disciplinaRepository,
            ISalaDeAulaRepository salaDeAulaRepository,
            IProfessorRepository professorRepository,
            IHorarioRepository horarioRepository,
            IMapper mapper) // <--- Adicione no construtor
        {
            _turmaRepository = turmaRepository;
            _disciplinaRepository = disciplinaRepository;
            _salaDeAulaRepository = salaDeAulaRepository;
            _professorRepository = professorRepository;
            _horarioRepository = horarioRepository;
            _mapper = mapper;
        }

        public async Task<TurmaResponseDto> CreateTurmaAsync(CreateTurmaRequestDto turmaDto)
        {
            await ValidateForeignKeys(turmaDto.DisciplinaID, turmaDto.SalaDeAulaID, turmaDto.ProfessorID, turmaDto.HorarioID);

            // ANTES: Mapeamento Manual (Várias linhas)
            // AGORA: Uma linha mágica
            var turmaEntidade = _mapper.Map<Turma>(turmaDto);

            var novoId = await _turmaRepository.AddAsync(turmaEntidade);
            turmaEntidade.TurmaID = novoId;

            // Retorna DTO mapeado automaticamente
            return _mapper.Map<TurmaResponseDto>(turmaEntidade);
        }

        public async Task<IEnumerable<TurmaResponseDto>> GetAllTurmasAsync()
        {
            var turmas = await _turmaRepository.GetAllAsync();
            // Converte a lista inteira de uma vez
            return _mapper.Map<IEnumerable<TurmaResponseDto>>(turmas);
        }

        public async Task<TurmaResponseDto?> GetTurmaByIdAsync(int id)
        {
            var turma = await _turmaRepository.GetByIdAsync(id);
            if (turma == null) return null;

            return _mapper.Map<TurmaResponseDto>(turma);
        }

        public async Task<bool> UpdateTurmaAsync(int id, UpdateTurmaRequestDto turmaDto)
        {
            var turmaExistente = await _turmaRepository.GetByIdAsync(id);
            if (turmaExistente == null) return false;

            await ValidateForeignKeys(turmaDto.DisciplinaID, turmaDto.SalaDeAulaID, turmaDto.ProfessorID, turmaDto.HorarioID);

            _mapper.Map(turmaDto, turmaExistente);

            turmaExistente.TurmaID = id;

            return await _turmaRepository.UpdateAsync(turmaExistente);
        }

        public async Task<bool> DeleteTurmaAsync(int id)
        {
            var turma = await _turmaRepository.GetByIdAsync(id);
            if (turma == null) return false;

            return await _turmaRepository.DeleteAsync(id);
        }

        private async Task ValidateForeignKeys(int disciplinaId, int salaId, int professorId, int horarioId)
        {
            if (await _disciplinaRepository.GetByIdAsync(disciplinaId) == null)
                throw new ArgumentException($"Disciplina com ID {disciplinaId} não encontrada.");

            if (await _salaDeAulaRepository.GetByIdAsync(salaId) == null)
                throw new ArgumentException($"Sala de Aula com ID {salaId} não encontrada.");

            if (await _professorRepository.GetByIdAsync(professorId) == null)
                throw new ArgumentException($"Professor com ID {professorId} não encontrado.");

            if (await _horarioRepository.GetByIdAsync(horarioId) == null)
                throw new ArgumentException($"Horário com ID {horarioId} não encontrado.");
        }

        
    }
}