using Model;
using Model.Ports;

namespace Persistence;

public class ArticuloRepository : IArticuloRepository
{
    private readonly List<Articulo> _articulos = new();

    public Task<Articulo?> GetByIdAsync(int id) =>
        Task.FromResult(_articulos.FirstOrDefault(a => a.Id == id));

    public Task<IEnumerable<Articulo>> GetAllAsync() =>
        Task.FromResult<IEnumerable<Articulo>>(_articulos);

    public Task AddAsync(Articulo articulo)
    {
        _articulos.Add(articulo);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Articulo articulo)
    {
        var idx = _articulos.FindIndex(a => a.Id == articulo.Id);
        if (idx >= 0) _articulos[idx] = articulo;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        var articulo = _articulos.FirstOrDefault(a => a.Id == id);
        if (articulo != null) _articulos.Remove(articulo);
        return Task.CompletedTask;
    }
}