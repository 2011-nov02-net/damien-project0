using System.Collections.Generic;

using StoreManager.Library.Data;
using StoreManager.Library.Entity;

namespace StoreManager.Library.Factory
{
    internal interface IFactory<T>
        where T : SEntity
    {
        List<T> Items { get; set; }

        int Create(IData data);
        T Get(int id);
        void Update(int id, IData data);
        void Delete(int id);
    }
}
