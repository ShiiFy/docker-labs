namespace OrderService.Models;

public class Order
{
    public int Id { get; set; }
    public int ProductId { get; set; }          // ID товара из ProductService
    public int Quantity { get; set; }            // Количество товара
    public string CustomerName { get; set; } = string.Empty;  // Имя покупателя
    public string Status { get; set; } = string.Empty;        // Статус заказа
}
