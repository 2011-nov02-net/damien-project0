using System;
using System.Collections.Generic;
using System.Text;

using ArkhManufacturing.Library.CreationData;

namespace ArkhManufacturing.Library.Creator
{
    public class LocationCreator : ICreator<Identifiable>
    {
        public Identifiable Create(ICreationData creationData) {
            if (creationData is LocationCreationData) {
                var data = creationData as LocationCreationData;
                return new Location(data.Planet, data.Province, data.City);
            } else throw new ArgumentException($"Expected LocationCreationData, but got '{creationData.GetType()}'.");
        }
    }
}
