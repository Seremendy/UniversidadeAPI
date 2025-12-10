using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories.Interfaces;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DisciplinasController : ControllerBase
{
    private readonly IDisciplinaRepository _repository;
    public DisciplinasController(IDisciplinaRepository repository) { _repository = repository; }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Disciplina>>> GetAll() => Ok(await _repository.GetAllAsync());

    [HttpPost]
    public async Task<ActionResult> Create(Disciplina disciplina)
    {
        await _repository.AddAsync(disciplina);
        return Ok();
    }
}