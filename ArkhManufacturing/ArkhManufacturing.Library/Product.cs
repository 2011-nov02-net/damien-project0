namespace ArkhManufacturing.Library
{
    public class Product : IdentifiableBase
    {
        private static readonly IdGenerator _idGenerator = new IdGenerator();

        public string Name { get; set; }
        public double? Discount { get; set; }

        private double _price;
        public double Price
        {
            get => _price * (Discount ?? 1);
            set => _price = value > 0 ? value : _price;
        }

        public Product(string name, double price, double? discount = null) :
            base(_idGenerator) {
            Name = name;
            Discount = discount;
            _price = price;
        }

        public override string ToString() => $"Product: Name: {Name}, Price: ${_price}{(Discount.HasValue ? $" at a {Discount * 100}% discount" : "")}";
    }
}
