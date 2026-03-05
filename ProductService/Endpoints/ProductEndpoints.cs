using ProductService.Models;

namespace ProductService.Endpoints;

public static class ProductEndpoints
{
    private static readonly List<Product> Products = new()
    {
        new Product
        {
            Id = 1,
            Name = "Игровой ноутбук ASUS ROG",
            Price = 120000,
            Stock = 5,
            Category = "Ноутбук",
            Specs = new Specs
            {
                Cpu = "Intel Core i7-12700H",
                Ram = 16,
                Storage = "1TB NVMe SSD",
                Gpu = "NVIDIA RTX 3070 8GB"
            }
        },
        new Product
        {
            Id = 2,
            Name = "Офисный ноутбук Lenovo",
            Price = 55000,
            Stock = 10,
            Category = "Ноутбук",
            Specs = new Specs
            {
                Cpu = "Intel Core i5-1235U",
                Ram = 8,
                Storage = "256GB SSD",
                Gpu = "Intel Iris Xe Graphics"
            }
        },
        new Product
        {
            Id = 3,
            Name = "Игровой компьютер",
            Price = 150000,
            Stock = 3,
            Category = "Компьютер",
            Specs = new Specs
            {
                Cpu = "AMD Ryzen 9 7900X",
                Ram = 32,
                Storage = "2TB NVMe SSD",
                Gpu = "NVIDIA RTX 4080 16GB"
            }
        },
        new Product
        {
            Id = 4,
            Name = "Офисный компьютер",
            Price = 45000,
            Stock = 8,
            Category = "Компьютер",
            Specs = new Specs
            {
                Cpu = "Intel Core i3-12100",
                Ram = 8,
                Storage = "256GB SSD",
                Gpu = "Intel UHD Graphics 730"
            }
        }
    };

    public static void MapProductEndpoints(this WebApplication app)
    {
        app.MapGet("/products", () => Results.Ok(Products));

        app.MapGet("/products/category/{category}", (string category) =>
        {
            var filtered = Products.Where(p => p.Category == category).ToList();
            return filtered.Count > 0
                ? Results.Ok(filtered)
                : Results.NotFound($"Товары категории '{category}' не найдены");
        });

        app.MapGet("/products/{id}", (int id) =>
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            return product is not null
                ? Results.Ok(product)
                : Results.NotFound($"Товар с ID {id} не найден");
        });

        app.MapPost("/products", (Product newProduct) =>
        {
            newProduct.Id = Products.Count > 0 ? Products.Max(p => p.Id) + 1 : 1;
            Products.Add(newProduct);
            return Results.Created($"/products/{newProduct.Id}", newProduct);
        });

        app.MapPut("/products/{id}", (int id, Product updatedProduct) =>
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product is null)
                return Results.NotFound($"Товар с ID {id} не найден");

            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;
            product.Stock = updatedProduct.Stock;
            product.Category = updatedProduct.Category;
            product.Specs = updatedProduct.Specs;
            return Results.Ok(product);
        });

        app.MapDelete("/products/{id}", (int id) =>
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product is null)
                return Results.NotFound($"Товар с ID {id} не найден");

            Products.Remove(product);
            return Results.Ok($"Товар '{product.Name}' удалён");
        });
    }
}
