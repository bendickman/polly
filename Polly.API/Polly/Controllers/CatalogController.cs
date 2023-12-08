using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Polly.Controllers;

[ApiController]
[Route("api/catalog")]
public class CatalogController : Controller
{
    private readonly HttpClient _client;

    public CatalogController(
        IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("InventoryClient");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(
        int id)
    {
        var response = await _client.GetAsync($"api/inventory/{id}");

        if (response.IsSuccessStatusCode)
        {
            var totalStock = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());

            return Ok(totalStock);
        }

        return StatusCode((int)response.StatusCode, response.Content.ReadAsStringAsync());
    }
}
