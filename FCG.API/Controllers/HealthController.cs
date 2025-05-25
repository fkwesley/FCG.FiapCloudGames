using Microsoft.AspNetCore.Mvc;

namespace FCG.FiapCloudGames.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpPost(Name = "Health")]
        public IActionResult Health()
        {
            return Ok();
        }
    }
}
