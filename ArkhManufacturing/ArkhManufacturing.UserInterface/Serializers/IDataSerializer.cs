using System;
using System.Collections.Generic;
using System.Text;

namespace ArkhManufacturing.UserInterface.Serializers
{
    // TODO: Add comment here
    public interface IDataSerializer<T>
    {
        // TODO: Add comment here
        void Write(T data);
        // TODO: Add comment here
        T Read();
    }
}
