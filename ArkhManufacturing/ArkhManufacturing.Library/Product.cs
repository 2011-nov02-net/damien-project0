using System;

namespace ArkhManufacturing.Library
{
    // TODO: Add comment here
    public class Product : Identifiable
    {
        private static readonly IdGenerator _idGenerator = new IdGenerator();

        // TODO: Add comment here
        public string Name { get; set; }
        // TODO: Add comment here
        public double? Discount { get; set; }

        private double _price;
        // TODO: Add comment here
        public double Price
        {
            get => _price * (Discount ?? 1);
            set => _price = value > 0 ? value : _price;
        }

        // TODO: Add comment here
        internal Product(string name, double price, double? discount = null) :
            base(_idGenerator) {
            Name = name;
            Discount = discount;
            _price = price;
        }

        // TODO: Add comment here
        public override string ToString() => $"Product: Name: {Name}, Price: ${_price}{(Discount.HasValue ? $" at a {Discount * 100}% discount" : "")}";

        public override bool Equals(object obj) {
            if (obj is Product) {
                return (obj as Product).Id == Id;
            } else return false;
        }

        public override int GetHashCode() => HashCode.Combine(Id, Name, Discount, _price, Price);
    }
}
