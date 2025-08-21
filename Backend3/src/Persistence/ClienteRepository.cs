using Model;
using Model.Ports;

namespace Persistence;

public class ClienteRepository : IClienteRepository
{
    private readonly List<Cliente> _clientes = new();

    public Task<Cliente?> GetByIdAsync(int id) =>
        Task.FromResult(_clientes.FirstOrDefault(c => c.Id == id));

    public Task<IEnumerable<Cliente>> GetAllAsync() =>
        Task.FromResult<IEnumerable<Cliente>>(_clientes);

    public Task AddAsync(Cliente cliente)
    {
        _clientes.Add(cliente);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Cliente cliente)
    {
        var idx = _clientes.FindIndex(c => c.Id == cliente.Id);
        if (idx >= 0) _clientes[idx] = cliente;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        var cliente = _clientes.FirstOrDefault(c => c.Id == id);
        if (cliente != null) _clientes.Remove(cliente);
        return Task.CompletedTask;
    }
}