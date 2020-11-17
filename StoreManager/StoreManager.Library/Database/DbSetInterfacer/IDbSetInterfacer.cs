using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManager.Library.Database.DbSetTranslator
{
    internal interface IDbSetInterfacer<T>
    {
        Task Create(T item);
        Task<T> Get(int id);
        Task Update(T item);
        Task Delete(T item);
    }
}
