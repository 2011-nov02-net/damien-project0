using System;

namespace ArkhManufacturing.Library.Exception
{
    // TODO: Add comment here
    public class ProductNotOfferedException : System.Exception
    {
        // TODO: Add comment here
        public ProductNotOfferedException(Product product) :
            base($"Product#{product.Id} ({product.Name}) was requested; none were in stock.") {
        }
    }
}
