using API.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ApiBaseController
{
    /// <summary>
    /// Health check para monitoramento da API.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        return StandardOk(new { status = "Healthy", application = "Publiseo" });
    }
}
