using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    abstract class NetworkObject
    {
        private static List<NetworkObject> allNetworkObjects = new List<NetworkObject>();
        private readonly Component[] components;
        public NetworkObject(Component[] _components)
        {
            allNetworkObjects.Add(this);
            components = _components;
        }

        // This method is virtual so component update order can be overridden.
        protected virtual void UpdateComponents()
        {
            foreach (Component _component in components)
            {
                _component.Update();
            }
        }

        public static void UpdateAll()
        {
            foreach (NetworkObject _networkObject in allNetworkObjects)
            {
                _networkObject.UpdateComponents();
            }
        }
    }
}
