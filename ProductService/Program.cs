using ProductService.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();

var app = builder.Build();

// Подключаем все маршруты из ProductEndpoints.cs
app.MapProductEndpoints();

app.Run();