using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Ports;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProveedoresController : ControllerBase
{
    private readonly IProveedorRepository _repo;
    public ProveedoresController(IProveedorRepository repo) => _repo = repo;

    [HttpGet]
    public async Task<IEnumerable<Proveedor>> Get() => await _repo.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Proveedor>> Get(int id)
    {
        var proveedor = await _repo.GetByIdAsync(id);
        return proveedor is null ? NotFound() : Ok(proveedor);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Proveedor proveedor)
    {
        await _repo.AddAsync(proveedor);
        return CreatedAtAction(nameof(Get), new { id = proveedor.Id }, proveedor);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Proveedor proveedor)
    {
        if (id != proveedor.Id) return BadRequest();
        await _repo.UpdateAsync(proveedor);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repo.DeleteAsync(id);
        return NoContent();
    }
}