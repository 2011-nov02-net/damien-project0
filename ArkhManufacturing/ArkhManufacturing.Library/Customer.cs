using System;

namespace ArkhManufacturing.Library
{
    // TODO: Add comment here
    public class Customer : IdentifiableBase
    {
        protected static readonly IdGenerator _idGenerator = new IdGenerator();

        // TODO: Add comment here
        public long DefaultStoreLocation { get; set; }
        // TODO: Add comment here
        public CustomerName Name { get; set; }

        // TODO: Add comment here
        public Customer(CustomerName name, long defaultStoreLocation) :
            base(_idGenerator) {
            DefaultStoreLocation = defaultStoreLocation;
            Name = name;
        }

        // TODO: Add comment here
        public override string ToString() {
            string defaultStoreLocationString = $" with Default Store Location ID#{DefaultStoreLocation}" ?? "";
            return $"Customer#{Id}: '{Name}'{defaultStoreLocationString}";
        }

        // TODO: Add comment here
        public override bool Equals(object obj) {
            if (obj is Customer)
                return (obj as Customer).Id == Id;
            else return false;
        }

        // TODO: Add comment here
        public override int GetHashCode() {
            return HashCode.Combine(Id, DefaultStoreLocation, Name);
        }
    }
}
