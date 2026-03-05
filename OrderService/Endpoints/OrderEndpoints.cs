using OrderService.Models;

namespace OrderService.Endpoints;

public static class OrderEndpoints
{
    private static readonly List<Order> Orders = new()
    {
        new Order { Id = 1, ProductId = 1, Quantity = 2, CustomerName = "Иван Иванов", Status = "Новый" },
        new Order { Id = 2, ProductId = 2, Quantity = 1, CustomerName = "Мария Петрова", Status = "Новый" }
    };

    private static readonly string ProductServiceUrl = "http://productservice:8080";

    public static void MapOrderEndpoints(this WebApplication app)
    {
        app.MapGet("/orders", () => Results.Ok(Orders));

        app.MapGet("/orders/{id}", (int id) =>
        {
            var order = Orders.FirstOrDefault(o => o.Id == id);
            return order is not null
                ? Results.Ok(order)
                : Results.NotFound($"Заказ с ID {id} не найден");
        });

        app.MapPost("/orders", async (Order newOrder, IHttpClientFactory httpClientFactory) =>
        {
            var client = httpClientFactory.CreateClient();

            var response = await client.GetAsync($"{ProductServiceUrl}/products/{newOrder.ProductId}");

            if (!response.IsSuccessStatusCode)
                return Results.BadRequest($"Товар с ID {newOrder.ProductId} не найден в ProductService");

            newOrder.Id = Orders.Count > 0 ? Orders.Max(o => o.Id) + 1 : 1;
            newOrder.Status = "Новый";
            Orders.Add(newOrder);
            return Results.Created($"/orders/{newOrder.Id}", newOrder);
        });

        app.MapPut("/orders/{id}", (int id, Order updatedOrder) =>
        {
            var order = Orders.FirstOrDefault(o => o.Id == id);
            if (order is null)
                return Results.NotFound($"Заказ с ID {id} не найден");

            order.ProductId = updatedOrder.ProductId;
            order.Quantity = updatedOrder.Quantity;
            order.CustomerName = updatedOrder.CustomerName;
            order.Status = updatedOrder.Status;
            return Results.Ok(order);
        });

        app.MapDelete("/orders/{id}", (int id) =>
        {
            var order = Orders.FirstOrDefault(o => o.Id == id);
            if (order is null)
                return Results.NotFound($"Заказ с ID {id} не найден");

            Orders.Remove(order);
            return Results.Ok($"Заказ с ID {id} удалён");
        });
    }
}
