using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomRoleBasedAuthorization.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecureController : ControllerBase
    {
        [CustomAuthorize("Admin")]
        [HttpGet("admin")]
        public IActionResult AdminEndpoint()
        {
            return Ok("Welcome, Admin!");
        }

        [CustomAuthorize("User")]
        [HttpGet("user")]
        public IActionResult UserEndpoint()
        {
            return Ok("Welcome, User!");
        }
    }
}
