using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    abstract class Component
    {
        private NetworkObject networkObject;
        public Component(NetworkObject _networkObject)
        {
            networkObject = _networkObject;
        }
        public abstract void Update();
    }
}
