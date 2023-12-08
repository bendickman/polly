using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Polly Retry Policy
IAsyncPolicy<HttpResponseMessage> retryPolicy =
    Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
    .RetryAsync(4, onRetry: (response, retryCount) =>
    {
        Console.WriteLine($"Retrying API call {retryCount}");
    });

builder.Services.AddSingleton(retryPolicy);

builder.Services.AddHttpClient("InventoryClient", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://localhost:7169/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
