using System;
using System.Collections.Generic;
using System.Text;

namespace ArkhManufacturing.Library.Exception
{
    // TODO: Add comment here
    public class NonExistentIndentifiableException : System.Exception
    {
        // TODO: Add comment here
        public NonExistentIndentifiableException(long itemId) :
            base($"Item with ID#{itemId} does not exist.") {
        }
    }
}
