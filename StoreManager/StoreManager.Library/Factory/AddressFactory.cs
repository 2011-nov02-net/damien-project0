using System;
using System.Collections.Generic;
using System.Linq;

using StoreManager.Library.Data;
using StoreManager.Library.Entity;

namespace StoreManager.Library.Factory
{
    internal class AddressFactory : IFactory<Address>
    {
        private readonly IdGenerator _idGenerator;

        public AddressFactory() {
            Items = new List<Address>();
            _idGenerator = new IdGenerator(0);
        }

        public AddressFactory(List<Address> addresses) {
            Items = addresses;
            _idGenerator = new IdGenerator(addresses.Max(a => a.Id));
        }

        public List<Address> Items { get; set; }

        public void Create(IData data) {
            var address = new Address(_idGenerator, data as AddressData);
            Items.Add(address);
        }

        public Address Get(long id) {
            return Items.FirstOrDefault(a => a.Id == id);
        }

        public void Update(long id, IData data) {
            var address = Get(id);
            
            if (address is null)
                return;
            
            address.Data = data as AddressData;
        }

        public void Delete(long id) {
            var address = Get(id);
            
            if (address is null)
                return;

            Items.Remove(address);
        }
    }
}
