using System;
using System.Collections.Generic;
using System.Text;

namespace ArkhManufacturing.Library.Exceptions
{
    public class ProductNotOfferedException : Exception
    {
        public ProductNotOfferedException(Product product) :
            base($"Product#{product.Id} ({product.Name}) was requested; none were in stock.") {
        }
    }
}
