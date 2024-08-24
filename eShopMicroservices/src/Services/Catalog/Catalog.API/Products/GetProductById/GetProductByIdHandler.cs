using Catalog.API.Exceptions;
namespace Catalog.API.Products.GetProductById;
public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
public record GetProductByIdResult(Product Product);

internal class GetProductByIdHandler(IDocumentSession documentSession, ILogger<GetProductByIdHandler> logger)
            : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken) 
    {
        logger.LogInformation($"GetProdyctById Handler class started with {query}.");

        var product = await documentSession.LoadAsync<Product>(query.Id);
        if (product == null)
        {
            logger.LogError($"Product with Id {query.Id} not found");
            throw new ProductNotFoundException("Product not found!");
        }
        return new GetProductByIdResult(product);
    }
}
