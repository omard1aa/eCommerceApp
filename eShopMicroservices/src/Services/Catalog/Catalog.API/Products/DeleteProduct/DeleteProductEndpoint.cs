namespace Catalog.API.Products.DeleteProduct;

// public record DeleteProductRequest(Guid Id);
public record DeleteProductResponse(bool IsDeleted);

public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{Id}", async (Guid Id, ISender sender) =>
        {
            var response = await sender.Send(new DeleteProductCommand(Id));

            var result = response.Adapt<DeleteProductResponse>();

            return Results.NoContent();
        })
        .WithName("DeleteProduct")
        .Produces<DeleteProductResponse>(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Product by Id deleted")
        .WithDescription("The product has been deleted.");
    }
}
