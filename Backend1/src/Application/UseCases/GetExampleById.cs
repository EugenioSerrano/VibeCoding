using Domain.Entities;
using Domain.Repositories;

namespace Application.UseCases {
    public class GetExampleById {
        private readonly IExampleRepository _repo;
        public GetExampleById(IExampleRepository repo) {
            _repo = repo;
        }
        public ExampleEntity Execute(int id) => _repo.GetById(id);
    }
}