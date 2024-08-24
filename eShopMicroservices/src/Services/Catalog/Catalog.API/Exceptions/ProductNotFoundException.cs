namespace Catalog.API.Exceptions;
public class ProductNotFoundException : Exception
{
    public ProductNotFoundException(string exception) : base(exception) { }
}

