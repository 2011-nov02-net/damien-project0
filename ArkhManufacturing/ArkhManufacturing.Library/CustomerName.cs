namespace ArkhManufacturing.Library
{
    public class CustomerName
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public CustomerName(string firstName, string lastName) {
            FirstName = firstName;
            LastName = lastName;
        }

        public override string ToString() => $"{LastName}, {FirstName}";
    }
}
