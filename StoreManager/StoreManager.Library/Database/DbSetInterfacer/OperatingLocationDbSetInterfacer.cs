using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DbOperatingLocation = StoreManager.DataModel.OperatingLocation;

using StoreManager.Library.Entity;

namespace StoreManager.Library.Database.DbSetTranslator
{
    internal class OperatingLocationDbSetInterfacer : IDbSetInterfacer<OperatingLocation>
    {
        public async Task Create(OperatingLocation item) {
            throw new NotImplementedException();
        }

        public async Task Delete(OperatingLocation item) {
            throw new NotImplementedException();
        }

        public async Task<OperatingLocation> Get(int id) {
            throw new NotImplementedException();
        }

        public async Task Update(OperatingLocation item) {
            throw new NotImplementedException();
        }
    }
}
