namespace ArkhManufacturing.Library.CreationData
{
    // TODO: Add comment here
    public class CustomerCreationData : ICreationData
    {
        // TODO: Add comment here
        public string FirstName { get; }
        // TODO: Add comment here
        public string LastName { get; }
        // TODO: Add comment here
        public Location DefaultStoreLocation { get; }

        // TODO: Add comment here
        public CustomerCreationData(string firstName, string lastName, Location defaultStoreLocation) {
            FirstName = firstName;
            LastName = lastName;
            DefaultStoreLocation = defaultStoreLocation;
        }
    }
}
