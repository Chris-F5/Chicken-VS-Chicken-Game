using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    abstract class ObjectTemplate
    {
        public readonly short typeId;
        public ObjectTemplate(NetworkObjectType _typeId)
        {
            typeId = (short)_typeId;
        }
        public abstract void AddComponentsToArray(NetworkObject _objectReference, ref Component[] _componentArray);
        public NetworkObject CreateObjectInstance()
        {
            return new NetworkObject(this);
        }
    }
}
