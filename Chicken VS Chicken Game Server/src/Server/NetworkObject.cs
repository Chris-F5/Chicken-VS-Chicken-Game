using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    enum NetworkObjectType
    {
        player,
        wall,
        testObject
    }
    abstract class NetworkObject
    {
        private static List<NetworkObject> allNetworkObjects = new List<NetworkObject>();

        public readonly short objectId;
        public readonly short objectTypeId;

        private readonly Component[] components;

        public NetworkObject(NetworkObjectType _ojectType, Component[] _components)
        {
            allNetworkObjects.Add(this);
            components = _components;
            objectTypeId = (short)_ojectType;

            // Sellect object id that is not in use.

            List<short> _takenObjectIds = new List<short>();

            foreach (NetworkObject _object in allNetworkObjects)
            {
                _takenObjectIds.Add(_object.objectId);
            }

            for (short i = 0; i <= short.MaxValue; i++)
            {
                if (!_takenObjectIds.Contains(i))
                {
                    objectId = i;
                    break;
                }
            }
            if (objectId == short.MaxValue)
            {
                Console.WriteLine($"CRITICAL WARNING : max NetworkSynchroniser count reached ({short.MaxValue}) reached - ignoring this NetworkSynchroniser");
                return;
            }
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
