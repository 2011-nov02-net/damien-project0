using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DbCustomer = StoreManager.DataModel.Customer;

using StoreManager.Library.Entity;

namespace StoreManager.Library.Database.DbSetTranslator
{
    internal class CustomerDbSetInterfacer : IDbSetInterfacer<Customer>
    {
        public async Task Create(Customer item) {
            throw new NotImplementedException();
        }

        public async Task Delete(Customer item) {
            throw new NotImplementedException();
        }

        public async Task<Customer> Get(int id) {
            throw new NotImplementedException();
        }

        public async Task Update(Customer item) {
            throw new NotImplementedException();
        }
    }
}
