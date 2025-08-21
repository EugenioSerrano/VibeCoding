using Backend2.Models;

namespace Backend2.Services
{
    public class ProductoService
    {
        private List<Producto> productos = new List<Producto>
        {
            new Producto { Id = 1, Nombre = "Laptop", Precio = 1200, Stock = 10 },
            new Producto { Id = 2, Nombre = "Mouse", Precio = 25, Stock = 100 },
            new Producto { Id = 3, Nombre = "Teclado", Precio = 45, Stock = 50 }
        };
        private int nextId = 4;

        public Task<List<Producto>> GetAllAsync() => Task.FromResult(productos.ToList());
        public Task<Producto?> GetByIdAsync(int id) => Task.FromResult(productos.FirstOrDefault(p => p.Id == id));
        public Task AddAsync(Producto producto)
        {
            producto.Id = nextId++;
            productos.Add(producto);
            return Task.CompletedTask;
        }
        public Task UpdateAsync(Producto producto)
        {
            var idx = productos.FindIndex(p => p.Id == producto.Id);
            if (idx >= 0) productos[idx] = producto;
            return Task.CompletedTask;
        }
        public Task DeleteAsync(int id)
        {
            productos.RemoveAll(p => p.Id == id);
            return Task.CompletedTask;
        }
    }
}