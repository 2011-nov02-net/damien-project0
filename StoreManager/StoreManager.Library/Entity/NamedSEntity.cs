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

        internal NamedSEntity(int id) :
            base(id) {
        }

        internal abstract string GetName();
    }
}
