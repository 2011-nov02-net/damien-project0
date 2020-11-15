namespace StoreManager.Library.Data
{
    public class ProductData : IData
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }

        public ProductData() { }

        public ProductData(string name, decimal price, decimal? discount) {
            Name = name;
            Price = price;
            Discount = discount;
        }

        public ProductData(ProductData data) :
            this(data.Name, data.Price, data.Discount) {
        }
    }
}
