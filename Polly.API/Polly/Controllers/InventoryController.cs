using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Polly.Controllers;

[ApiController]
[Route("api/inventory")]
public class InventoryController : Controller
{
    private static int _requestCount = 0;

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(
        int id)
    {
        await Task.Delay(100);

        _requestCount++;

        if (_requestCount % 4 == 0) //ensure only 1 in 4 calls will be successful
        {
            return Ok(10);
        }

        return StatusCode((int)HttpStatusCode.InternalServerError, "Server error");
    }
}
