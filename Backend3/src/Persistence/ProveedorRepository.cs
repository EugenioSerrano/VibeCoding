using Model;
using Model.Ports;

namespace Persistence;

public class ProveedorRepository : IProveedorRepository
{
    private readonly List<Proveedor> _proveedores = new();

    public Task<Proveedor?> GetByIdAsync(int id) =>
        Task.FromResult(_proveedores.FirstOrDefault(p => p.Id == id));

    public Task<IEnumerable<Proveedor>> GetAllAsync() =>
        Task.FromResult<IEnumerable<Proveedor>>(_proveedores);

    public Task AddAsync(Proveedor proveedor)
    {
        _proveedores.Add(proveedor);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Proveedor proveedor)
    {
        var idx = _proveedores.FindIndex(p => p.Id == proveedor.Id);
        if (idx >= 0) _proveedores[idx] = proveedor;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        var proveedor = _proveedores.FirstOrDefault(p => p.Id == id);
        if (proveedor != null) _proveedores.Remove(proveedor);
        return Task.CompletedTask;
    }
}