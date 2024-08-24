using Catalog.API.Exceptions;
using Catalog.API.Models;

namespace Catalog.API.Products.GetProductByCategory;

public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;
public record GetProductByCategoryResult(IEnumerable<Product> Product);
internal class GetProductByCategoryHandler(IDocumentSession documentSession, ILogger<GetProductByCategoryHandler> logger)
    : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation($"GetProdyctByCategory Handler class started with {query}.");

        var products = await documentSession.Query<Product>()
            .Where(p => p.Category.Contains(query.Category))
            .ToListAsync();
        if (products == null)
        {
            logger.LogError($"Product with Id {query.Category} not found");
            throw new ProductNotFoundException("Product not found!");
        }
        return new GetProductByCategoryResult(products);
    }
}
