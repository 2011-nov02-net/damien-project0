using System.Collections.Generic;

using StoreManager.Library.Data;

namespace StoreManager.Library.Factory
{
    interface IFactory<T>
    {
        List<T> Items { get; set; }

        long Create(IData data);
        T Get(long id);
        void Update(long id, IData data);
        void Delete(long id);
    }
}
