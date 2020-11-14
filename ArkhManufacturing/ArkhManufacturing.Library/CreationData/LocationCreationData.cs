using System;
namespace ArkhManufacturing.Library.CreationData
{
    // TODO: Add comment here
    public class LocationCreationData : ICreationData
    {
        // TODO: Add comment here
        public LocationCreationData(string planet, string province, string city) {
            Planet = planet;
            Province = province;
            City = city;
        }

        // TODO: Add comment here
        public string Planet { get; }
        // TODO: Add comment here
        public string Province { get; }
        // TODO: Add comment here
        public string City { get; }

    }
}
