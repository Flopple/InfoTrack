using System.Diagnostics.CodeAnalysis;
using InfoTrack.Web.API.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace InfoTrack.Web.API.Controllers;

[ExcludeFromCodeCoverage]
[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Health()
    {
        return ApiResponseFactory.CreateSuccessResponse("API is up");
    }
}