namespace ArkhManufacturing.Library
{
    public class Customer : IdentifiableBase
    {
        protected static readonly IdGenerator _idGenerator = new IdGenerator();

        public Location DefaultStoreLocation { get; set; }
        public CustomerName Name { get; set; }

        public Customer(CustomerName name, Location defaultStoreLocation = null) :
            base(_idGenerator)
        {
            DefaultStoreLocation = defaultStoreLocation;
            Name = name;
        }

        public override string ToString() => $"Customer#{Id}: '{Name}'{(DefaultStoreLocation?.ToString() ?? "")}";
    }
}
