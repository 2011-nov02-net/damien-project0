using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StoreManager.Library.Entity;

using DbStore = StoreManager.DataModel.Store;

namespace StoreManager.Library.Database.DbSetTranslator
{
    internal class StoreDbSetInterfacer : IDbSetInterfacer<Store>
    {
        public void Create(Store item) {
            throw new NotImplementedException();
        }

        public void Delete(Store item) {
            throw new NotImplementedException();
        }

        public Store Get(int id) {
            throw new NotImplementedException();
        }

        public void Update(Store item) {
            throw new NotImplementedException();
        }
    }
}
