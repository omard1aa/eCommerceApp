namespace Catalog.API.Products.GetProducts;

public record GetProductsResponse(IEnumerable<Product> Products);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (ISender sender, ILogger<GetProductsEndpoint> logger) =>
        {
            var result = await sender.Send(new GetProductsQuery());

            var response = result.Adapt<GetProductsResponse>();
            if (response == null)
            {
                logger.LogError("Response adaptation failed");
                return Results.BadRequest("Failed to create response");
            }
            return Results.Ok(response);
        })
        .WithName("GetProduct")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Product fetched")
        .WithDescription("Products fetched with get endpoint.");
    }
}

