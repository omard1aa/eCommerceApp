namespace Catalog.API.Products.CreateProduct;

public record CreateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);

public record CreateProductResponse(Guid Id);

public class CreateProductEndpoint : ICarterModule
{
    private readonly ILogger<CreateProductEndpoint> _logger;
    public CreateProductEndpoint(ILogger<CreateProductEndpoint> logger)
    {
        _logger = logger;
    }
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductRequest request, ISender sender) => {
            _logger.LogInformation("Received request to create product");

            if (request == null)
            {
                _logger.LogError("Request is null");
                return Results.BadRequest("Invalid request");
            }

            _logger.LogInformation("Request is valid");

            if (sender == null)
            {
                _logger.LogError("ISender is not registered");
                throw new ArgumentNullException(nameof(sender), "ISender is not registered");
            }

            _logger.LogInformation("ISender is registered");

            var command = request.Adapt<CreateProductCommand>();
            if (command == null)
            {
                _logger.LogError("Command adaptation failed");
                return Results.BadRequest("Failed to create command");
            }

            _logger.LogInformation("Command adaptation succeeded");

            var result = await sender.Send(command);
            if (result == null)
            {
                _logger.LogError("Failed to create product");
                return Results.BadRequest("Failed to create product");
            }

            _logger.LogInformation("Product creation succeeded");

            var response = result.Adapt<CreateProductResponse>();
            if (response == null)
            {
                _logger.LogError("Response adaptation failed");
                return Results.BadRequest("Failed to create response");
            }

            _logger.LogInformation("Response adaptation succeeded");

            return Results.Created($"/products/{response.Id}", response);
        })
            .WithName("CreateProduct")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Product created")
            .WithDescription("New product created.");
    }
}
