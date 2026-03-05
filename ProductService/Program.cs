using ProductService.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();

var app = builder.Build();
app.MapProductEndpoints();

app.Run();