using System;
using System.Collections.Generic;
using System.Linq;
using StoreManager.Library.Data;
using StoreManager.Library.Entity;

namespace StoreManager.Library.Factory
{
    internal class OperatingLocationFactory : IFactory<OperatingLocation>
    {
        private readonly IdGenerator _idGenerator;

        public OperatingLocationFactory() {
            Items = new List<OperatingLocation>();
            _idGenerator = new IdGenerator(0);
        }

        public OperatingLocationFactory(List<OperatingLocation> operatingLocations) {
            Items = operatingLocations;
            _idGenerator = new IdGenerator(Items.Max(ol => ol.Id));
        }

        public List<OperatingLocation> Items { get; set; }

        public int Create(IData data) {
            var operatingLocation = new OperatingLocation(_idGenerator, data as OperatingLocationData);
            Items.Add(operatingLocation);
            return operatingLocation.Id;
        }

        public void Delete(int id) {
            var operatingLocation = Get(id);

            if (operatingLocation is null)
                return;

            Items.Remove(operatingLocation);
        }

        public OperatingLocation Get(int id) {
            return Items.FirstOrDefault(ol => ol.Id == id);
        }

        public void Update(int id, IData data) {
            var operatingLocation = Get(id);

            if (operatingLocation is null)
                return;

            operatingLocation.Data = data as OperatingLocationData;
        }
    }
}
