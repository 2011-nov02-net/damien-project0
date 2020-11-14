using System;
using System.Collections.Generic;
using System.Text;

namespace ArkhManufacturing.UserInterface.Serializers
{
    // TODO: Add comment here
    public interface IDataStorage<T>
    {
        // TODO: Add comment here
        void Commit(T data);
        // TODO: Add comment here
        T Read();
    }
}
