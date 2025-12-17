using AutoMapper;
using UniversidadeAPI.DTOs;
using UniversidadeAPI.Entities;

namespace UniversidadeAPI.Configuration
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateTurmaRequestDto, Turma>();
            CreateMap<UpdateTurmaRequestDto, Turma>();
            CreateMap<Turma, TurmaResponseDto>();

            CreateMap<CreateAlunoRequestDto, Aluno>();

            CreateMap<CreateDepartamentoRequestDto, Departamento>();
            CreateMap<Departamento, DepartamentoDtos>();

            CreateMap<CreateCursoRequestDto, Curso>();
            CreateMap<Curso, CursoResponseDto>();

            CreateMap<CreateDisciplinaRequestDto, Disciplina>();
            CreateMap<UpdateDisciplinaRequestDto, Disciplina>();
            CreateMap<Disciplina, DisciplinaResponseDto>();

            CreateMap<CreateGradeRequestDto, GradeCurricular>();
            CreateMap<GradeCurricular, GradeCurricularResponseDto>();

            CreateMap<CreateHorarioRequestDto, Horario>();

            CreateMap<Horario, HorarioResponseDto>()
                .ForMember(dest => dest.DiaSemana, opt => opt.MapFrom(src => src.DiaSemana.ToString()));

            CreateMap<CreateMatriculaRequestDto, Matricula>()
                .ForMember(dest => dest.DataMatricula, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.MatriculaAtiva, opt => opt.MapFrom(src => true));

            CreateMap<Matricula, MatriculaResponseDto>();

            CreateMap<CreateNotaRequestDto, Nota>();
            CreateMap<UpdateNotaRequestDto, Nota>();
            CreateMap<Nota, NotaResponseDto>();

            CreateMap<CreatePrerequisitoRequestDto, Prerequisito>();
            CreateMap<Prerequisito, PrerequisitoResponseDto>();

            CreateMap<CreateProfessorRequestDto, Professor>();
            CreateMap<UpdateProfessorRequestDto, Professor>();
            CreateMap<Professor, ProfessorResponseDto>();
        }
    }
}