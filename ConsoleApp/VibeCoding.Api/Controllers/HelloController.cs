using Microsoft.AspNetCore.Mvc;

namespace VibeCoding.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { message = "¡Hola desde VibeCoding API!" });
        }
    }
}
