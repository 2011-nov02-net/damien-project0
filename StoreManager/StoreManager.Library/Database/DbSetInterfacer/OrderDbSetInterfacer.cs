using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DbOrder = StoreManager.DataModel.Order;

using StoreManager.Library.Entity;

namespace StoreManager.Library.Database.DbSetTranslator
{
    internal class OrderDbSetInterfacer : IDbSetInterfacer<Order>
    {
        public async Task Create(Order item) {
            throw new NotImplementedException();
        }

        public async Task Delete(Order item) {
            throw new NotImplementedException();
        }

        public async Task<Order> Get(int id) {
            throw new NotImplementedException();
        }

        public async Task Update(Order item) {
            throw new NotImplementedException();
        }
    }
}
