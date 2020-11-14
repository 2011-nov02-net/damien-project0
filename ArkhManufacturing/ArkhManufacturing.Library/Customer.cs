using System;

namespace ArkhManufacturing.Library
{
    // TODO: Add comment here
    public class Customer : Identifiable
    {
        protected static readonly IdGenerator _idGenerator = new IdGenerator();

        // TODO: Add comment here
        public Location DefaultStoreLocation { get; set; }
        // TODO: Add comment here
        public string FirstName { get; set; }
        // TODO: Add comment here
        public string LastName { get; set; }
        public string FullName { get => $"{LastName}, {FirstName}"; }

        // TODO: Add comment here
        internal Customer(string firstName, string lastName, Location defaultStoreLocation) :
            base(_idGenerator) {
            FirstName = firstName;
            LastName = lastName;
            DefaultStoreLocation = defaultStoreLocation;
        }

        // TODO: Add comment here
        public override string ToString() {
            string defaultStoreLocationString = DefaultStoreLocation != null ? $" with Default Store Location ID#{DefaultStoreLocation}" : "";
            return $"Customer#{Id}: '{FullName}'{defaultStoreLocationString}";
        }

        // TODO: Add comment here
        public override bool Equals(object obj) {
            if (obj is Customer)
                return (obj as Customer).Id == Id;
            else return false;
        }

        // TODO: Add comment here
        public override int GetHashCode() => HashCode.Combine(Id, DefaultStoreLocation, FirstName, LastName);
    }
}
