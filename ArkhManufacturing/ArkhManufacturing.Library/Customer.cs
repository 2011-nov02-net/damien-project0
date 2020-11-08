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

        public bool HasDefaultStoreLocation() => DefaultStoreLocation != null;

        public override string ToString()
        {
            string defaultStoreLocationString = DefaultStoreLocation?.ToString() ?? "";
            return $"Customer#{Id}: '{Name}' {defaultStoreLocationString}";
        }
        
        public override bool Equals(object obj)
        {
            if (obj is Customer)
                return (obj as Customer).Id == Id;
            else return false;
        }
    }
}
