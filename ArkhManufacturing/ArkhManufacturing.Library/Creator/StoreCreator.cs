using System;
using System.Collections.Generic;
using System.Text;

using ArkhManufacturing.Library.CreationData;

namespace ArkhManufacturing.Library.Creator
{
    public class StoreCreator : ICreator<Identifiable>
    {
        public Identifiable Create(ICreationData creationData) {
            if (creationData is StoreCreationData) {
                var data = creationData as StoreCreationData;
                return new Store(data.Name, data.ProductCountThreshold, data.Location);
            } else throw new ArgumentException($"Expected StoreCreationData, but got '{creationData.GetType()}'.");
        }
    }
}
