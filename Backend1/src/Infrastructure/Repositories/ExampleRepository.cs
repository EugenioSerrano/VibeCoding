using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.Repositories {
    public class ExampleRepository : IExampleRepository {
        public ExampleEntity GetById(int id) {
            // Simulación de acceso a datos
            return new ExampleEntity { Id = id, Name = $"Name{id}" };
        }
    }
}