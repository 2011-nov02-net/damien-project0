using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StoreManager.Library.Data;

namespace StoreManager.Library
{
    public interface ISerializer
    {
        Task<DataBundle> ReadAsync();
        Task WriteAsync(DataBundle dataBundle);
    }
}
