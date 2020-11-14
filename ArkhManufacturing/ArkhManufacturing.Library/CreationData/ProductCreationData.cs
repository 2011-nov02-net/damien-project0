using System;
namespace ArkhManufacturing.Library.CreationData
{
    // TODO: Add comment here
    public class ProductCreationData : ICreationData
    {
        // TODO: Add comment here
        public ProductCreationData(string name, double price, double? discout) {
            Name = name;
            Price = price;
            Discout = discout;
        }

        // TODO: Add comment here
        public string Name { get; }
        // TODO: Add comment here
        public double Price { get; }
        // TODO: Add comment here
        public double? Discout { get; }
    }
}
