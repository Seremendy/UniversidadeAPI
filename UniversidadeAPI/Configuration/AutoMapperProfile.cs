using AutoMapper;
using UniversidadeAPI.DTOs;
using UniversidadeAPI.Entities;

namespace UniversidadeAPI.Configuration
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateTurmaRequestDto, Turmas>();
            CreateMap<UpdateTurmaRequestDto, Turmas>();
            CreateMap<Turmas, TurmaResponseDto>();

            CreateMap<CreateAlunoRequestDto, Alunos>();

            CreateMap<CreateDepartamentoRequestDto, Departamentos>();
            CreateMap<Departamentos, DepartamentoDtos>();

            CreateMap<CreateCursoRequestDto, Cursos>();
            CreateMap<Cursos, CursoResponseDto>();

            CreateMap<CreateDisciplinaRequestDto, Disciplinas>();
            CreateMap<UpdateDisciplinaRequestDto, Disciplinas>();
            CreateMap<Disciplinas, DisciplinaResponseDto>();

            CreateMap<CreateGradeRequestDto, GradeCurriculares>();
            CreateMap<GradeCurriculares, GradeCurricularResponseDto>();

            CreateMap<CreateHorarioRequestDto, Horarios>();

            CreateMap<Horarios, HorarioResponseDto>()
                .ForMember(dest => dest.DiaSemana, opt => opt.MapFrom(src => src.DiaSemana.ToString()));

            CreateMap<CreateMatriculaRequestDto, Matriculas>()
                .ForMember(dest => dest.DataMatricula, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.MatriculaAtiva, opt => opt.MapFrom(src => true));

            CreateMap<Matriculas, MatriculaResponseDto>();

            CreateMap<CreateNotaRequestDto, Notas>();
            CreateMap<UpdateNotaRequestDto, Notas>();
            CreateMap<Notas, NotaResponseDto>();

            CreateMap<CreatePrerequisitoRequestDto, Prerequisitos>();
            CreateMap<Prerequisitos, PrerequisitoResponseDto>();

            CreateMap<CreateProfessorRequestDto, Professores>();
            CreateMap<UpdateProfessorRequestDto, Professores>();
            CreateMap<Professores, ProfessorResponseDto>();
        }
    }
}