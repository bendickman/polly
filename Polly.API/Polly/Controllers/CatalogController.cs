using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Polly.Controllers;

[ApiController]
[Route("api/catalog")]
public class CatalogController : Controller
{
    private readonly HttpClient _client;
    private readonly IAsyncPolicy<HttpResponseMessage> _resiliencePolicy;

    public CatalogController(
        IHttpClientFactory httpClientFactory,
        IAsyncPolicy<HttpResponseMessage> resiliencePolicy)
    {
        _client = httpClientFactory.CreateClient("InventoryClient");
        _resiliencePolicy = resiliencePolicy;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(
        int id)
    {
        var response = await _resiliencePolicy.ExecuteAsync(() =>
            _client.GetAsync($"api/inventory/{id}"));

        if (response.IsSuccessStatusCode)
        {
            var totalStock = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());

            return Ok(totalStock);
        }

        return StatusCode((int)response.StatusCode, response.Content.ReadAsStringAsync());
    }
}
