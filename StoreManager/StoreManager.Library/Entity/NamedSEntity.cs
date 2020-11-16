using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManager.Library.Entity
{
    public abstract class NamedSEntity : SEntity
    {
        internal NamedSEntity(IdGenerator idGenerator) :
            base(idGenerator) {

        }

        internal abstract string GetName();
    }
}
