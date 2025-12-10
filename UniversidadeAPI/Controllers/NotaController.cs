using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversidadeAPI.Entities;
using UniversidadeAPI.Repositories.Interfaces;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotasController : ControllerBase
{
    private readonly INotaRepository _repository;
    public NotasController(INotaRepository repository) { _repository = repository; }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Nota>>> GetAll() => Ok(await _repository.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<Nota>> GetById(int id) => Ok(await _repository.GetByIdAsync(id));

    [HttpPost]
    public async Task<ActionResult> Create(Nota nota)
    {
        await _repository.AddAsync(nota);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, Nota nota)
    {
        if (id != nota.NotaID) return BadRequest();
        await _repository.UpdateAsync(nota);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _repository.DeleteAsync(id);
        return NoContent();
    }
}