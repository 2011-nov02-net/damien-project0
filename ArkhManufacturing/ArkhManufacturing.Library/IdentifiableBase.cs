using System;
using System.Collections.Generic;
using System.Text;

namespace ArkhManufacturing.Library
{
    public abstract class IdentifiableBase
    {        
        public long Id { get; private set; }

        public IdentifiableBase(IdGenerator idGenerator)
        {
            Id = idGenerator.NextId();
        }
    }
}
