namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<UpdateProductResult>;
public record UpdateProductResult(bool IsUpdated);
internal class UpdateProductHandler(IDocumentSession session, ILogger<UpdateProductHandler> logger)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var myProduct = await session.LoadAsync<Product>(command.Id);
        if(myProduct == null)
        {
            logger.LogWarning($"Product with Id {command.Id} not found!");
            return new UpdateProductResult(false);
        }
        myProduct.Name = command.Name;
        myProduct.Category = command.Category;
        myProduct.Description = command.Description;
        myProduct.ImageFile = command.ImageFile;
        myProduct.Price = command.Price;

        session.Update(myProduct);
        await session.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(true);
    }
}
