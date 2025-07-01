using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            // Örnek veri dön
            return Ok(new[] { "Görev 1", "Görev 2" });
        }
    }
}
