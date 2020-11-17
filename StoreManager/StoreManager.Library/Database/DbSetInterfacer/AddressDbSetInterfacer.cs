using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DbAddress = StoreManager.DataModel.Address;

using StoreManager.Library.Entity;

namespace StoreManager.Library.Database.DbSetTranslator
{
    internal class AddressDbSetInterfacer : IDbSetInterfacer<Address>
    {
        public async Task Create(Address item) {
            throw new NotImplementedException();
        }

        public async Task Delete(Address item) {
            throw new NotImplementedException();
        }

        public async Task<Address> Get(int id) {
            throw new NotImplementedException();
        }

        public async Task Update(Address item) {
            throw new NotImplementedException();
        }
    }
}
