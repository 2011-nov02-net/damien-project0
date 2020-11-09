using System;

namespace ArkhManufacturing.Library.Exceptions
{
    public class ProductRequestExcessiveException : Exception
    {
        public ProductRequestExcessiveException(Product product, int requestedCount, int inventoryCount) :
            base($"x{requestedCount} of Product#{product.Id} ({product.Name}) was requested; only {inventoryCount} are current in stock.") {
        }
    }
}
