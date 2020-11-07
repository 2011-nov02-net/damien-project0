using System;
using System.Collections.Generic;
using System.Text;

namespace ArkhManufacturing.UserInterface.Serializers
{
    public interface IDataSerializer<T>
    {
        void Write(T data);
        T Read();
    }
}
