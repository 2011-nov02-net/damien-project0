namespace ArkhManufacturing.Library
{
    public class Location : IdentifiableBase
    {
        protected static readonly IdGenerator _idGenerator = new IdGenerator();

        public string Planet { get; set; }
        public string Province { get; set; }
        public string City { get; set; }

        public Location(string planet, string province, string city) :
            base(_idGenerator)
        {
            Planet = planet;
            Province = province;
            City = city;
        }

        public override string ToString() => $"{City}, {Province}, {Planet}";
    }
}
