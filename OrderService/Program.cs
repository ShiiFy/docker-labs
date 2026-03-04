using OrderService.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();

var app = builder.Build();

// Подключаем все маршруты из OrderEndpoints.cs
app.MapOrderEndpoints();

app.Run();