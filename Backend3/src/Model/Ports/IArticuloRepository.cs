namespace Model.Ports;

public interface IArticuloRepository
{
    Task<Articulo?> GetByIdAsync(int id);
    Task<IEnumerable<Articulo>> GetAllAsync();
    Task AddAsync(Articulo articulo);
    Task UpdateAsync(Articulo articulo);
    Task DeleteAsync(int id);
}