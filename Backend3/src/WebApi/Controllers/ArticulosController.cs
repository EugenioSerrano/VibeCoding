using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Ports;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticulosController : ControllerBase
{
    private readonly IArticuloRepository _repo;
    public ArticulosController(IArticuloRepository repo) => _repo = repo;

    [HttpGet]
    public async Task<IEnumerable<Articulo>> Get() => await _repo.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Articulo>> Get(int id)
    {
        var articulo = await _repo.GetByIdAsync(id);
        return articulo is null ? NotFound() : Ok(articulo);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Articulo articulo)
    {
        await _repo.AddAsync(articulo);
        return CreatedAtAction(nameof(Get), new { id = articulo.Id }, articulo);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Articulo articulo)
    {
        if (id != articulo.Id) return BadRequest();
        await _repo.UpdateAsync(articulo);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repo.DeleteAsync(id);
        return NoContent();
    }
}