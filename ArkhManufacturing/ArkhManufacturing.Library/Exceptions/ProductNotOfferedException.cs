using System;

namespace ArkhManufacturing.Library.Exceptions
{
    // TODO: Add comment here
    public class ProductNotOfferedException : Exception
    {
        // TODO: Add comment here
        public ProductNotOfferedException(Product product) :
            base($"Product#{product.Id} ({product.Name}) was requested; none were in stock.") {
        }
    }
}
