namespace Catalog.API.Products.GetProductById;

// public record GetProductByIdRequest();
public record GetProductByIdResponse(Product product);
public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id}", async (Guid id, ISender sender, ILogger<GetProductByIdEndpoint> logger) =>
        {
            var result = await sender.Send(new GetProductByIdQuery(id));
            if (result == null) logger.LogWarning($"failed to fetch the product with Id {id}.");

            var response = result.Adapt<GetProductByIdResponse>();
            if (response.product == null) logger.LogWarning($"failed to map to Product with Id {id}.");

            return Results.Ok(response);
        })
        .WithName("GetProductById")
        .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Product by Id fetched")
        .WithDescription("The product has been fetched.");
    }
}

