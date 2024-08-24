
namespace Catalog.API.Products.GetProductByCategory;

public record GetProductsByCategoryRequest(string Category);
public record GetProductsByCategoryResponse(List<Product> Product);

public class GetProductsByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{Category}", async (string Category, ISender sender, ILogger<GetProductsByCategoryEndpoint> logger) =>
        {
            var response = await sender.Send(new GetProductByCategoryQuery(Category));
            if (response == null) logger.LogError($"Request the product with category {Category} hadn't completed.");

            var result = response.Adapt<GetProductsByCategoryResponse>();
            if (result == null) logger.LogError($"Product with category {Category} not mapped");

            return Results.Ok(result);
        })
        .WithName("GetProductByCategory")
        .Produces<GetProductsByCategoryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Products by Category fetched")
        .WithDescription("Products has been fetched.");
    }
}
