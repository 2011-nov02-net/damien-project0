namespace ArkhManufacturing.Library
{
    public class Customer : IdentifiableBase
    {
        protected static readonly IdGenerator _idGenerator = new IdGenerator();

        // TODO: Add comment here
        public Location DefaultStoreLocation { get; set; }
        // TODO: Add comment here
        public CustomerName Name { get; set; }

        // TODO: Add comment here
        public Customer(CustomerName name, Location defaultStoreLocation = null) :
            base(_idGenerator) {
            DefaultStoreLocation = defaultStoreLocation;
            Name = name;
        }

        // TODO: Add comment here
        public bool HasDefaultStoreLocation() => DefaultStoreLocation != null;

        // TODO: Add comment here
        public override string ToString() {
            string defaultStoreLocationString = DefaultStoreLocation?.ToString() ?? "";
            return $"Customer#{Id}: '{Name}' {defaultStoreLocationString}";
        }

        // TODO: Add comment here
        public override bool Equals(object obj) {
            if (obj is Customer)
                return (obj as Customer).Id == Id;
            else return false;
        }
    }
}
