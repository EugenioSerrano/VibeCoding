using Application.UseCases;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class ExampleController : ControllerBase {
        [HttpGet("{id}")]
        public IActionResult Get(int id) {
            var useCase = new GetExampleById(new ExampleRepository());
            var result = useCase.Execute(id);
            return Ok(result);
        }
    }
}