using System.Collections.Generic;

using StoreManager.Library.Data;
using StoreManager.Library.Entity;

namespace StoreManager.Library.Factory
{
    interface IFactory<T>
        where T : SEntity
    {
        List<T> Items { get; set; }

        long Create(IData data);
        T Get(long id);
        void Update(long id, IData data);
        void Delete(long id);
    }
}
