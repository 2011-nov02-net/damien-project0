using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StoreManager.Library.Data;
using StoreManager.Library.Entity;

namespace StoreManager.Library.Factory
{
    internal class StoreFactory : IFactory<Store>
    {
        private readonly IdGenerator _idGenerator;
        public List<Store> Items { get; set; }

        public StoreFactory() {
            Items = new List<Store>();
            _idGenerator = new IdGenerator(0);
        }

        public StoreFactory(List<Store> stores) {
            Items = stores;
            _idGenerator = new IdGenerator(Items.Max(s => s.Id));
        }

        public void Create(IData data) {
            var store = new Store(_idGenerator, data as StoreData);
            Items.Add(store);
        }

        public void Delete(long id) {
            var store = Get(id);

            if (store is null)
                return;

            Items.Remove(store);
        }

        public Store Get(long id) {
            return Items.FirstOrDefault(s => s.Id == id);
        }

        public void Update(long id, IData data) {
            var store = Get(id);

            if (store is null)
                return;

            store.Data = data as StoreData;
        }
    }
}
