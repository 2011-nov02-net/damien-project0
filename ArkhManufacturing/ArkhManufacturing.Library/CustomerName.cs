namespace ArkhManufacturing.Library
{
    public class CustomerName
    {
        // TODO: Add comment here
        public string FirstName { get; set; }
        // TODO: Add comment here
        public string LastName { get; set; }

        // TODO: Add comment here
        public CustomerName(string firstName, string lastName) {
            FirstName = firstName;
            LastName = lastName;
        }

        // TODO: Add comment here
        public override string ToString() => $"{LastName}, {FirstName}";
    }
}
