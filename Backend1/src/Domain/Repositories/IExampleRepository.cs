using Domain.Entities;
namespace Domain.Repositories {
    public interface IExampleRepository {
        ExampleEntity GetById(int id);
    }
}