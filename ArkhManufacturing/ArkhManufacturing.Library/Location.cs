using System;

namespace ArkhManufacturing.Library
{
    // TODO: Add comment here
    public class Location : IdentifiableBase
    {
        protected static readonly IdGenerator _idGenerator = new IdGenerator();

        // TODO: Add comment here
        public string Planet { get; set; }
        // TODO: Add comment here
        public string Province { get; set; }
        // TODO: Add comment here
        public string City { get; set; }

        // TODO: Add comment here
        public Location(string planet, string province, string city) :
            base(_idGenerator) {
            Planet = planet;
            Province = province;
            City = city;
        }

        // TODO: Add comment here
        public override string ToString() => $"{City}, {Province}, {Planet}";

        // TODO: Add comment here
        public override bool Equals(object obj) {
            if (obj is Location) {
                return (obj as Location).Id == Id;
            } else return false;
        }

        // TODO: Add comment here
        public override int GetHashCode() {
            return HashCode.Combine(Id, Planet, Province, City);
        }
    }
}
