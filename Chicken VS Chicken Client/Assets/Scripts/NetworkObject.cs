using UnityEngine;
using SharedClassLibrary.Networking;

namespace GameClient
{
    public class NetworkObject : MonoBehaviour
    {
        private NetworkObjectComponent[] components;
        public short objectId { get; protected set; }
        public bool startupInfoSent { get; private set; } = false;

        private void Awake()
        {
            components = gameObject.GetComponents<NetworkObjectComponent>();
        }

        public void HandleSynchronise(Packet _packet)
        {
            while (true)
            {
                byte _componentIndex = _packet.ReadByte();
                if (_componentIndex == 255)
                {
                    break;
                }
                else
                {
                    // If this component is the network object component
                    if (_componentIndex == 0)
                    {
                        HandleNetworkObjectComponentEvents(_packet);
                    }
                    else
                    {
                        Debug.Log(_componentIndex);
                        components[_componentIndex - 1].HandleSynchronise(_packet);
                    }
                }
            }
        }
        private void HandleNetworkObjectComponentEvents(Packet _packet)
        {
            while (true)
            {
                byte _eventId = _packet.ReadByte();
                if (_eventId == EventIds.EventEnd)
                {
                    break;
                }
                else
                {
                    switch (_eventId)
                    {
                        case EventIds.ObjectCreated:
                            startupInfoSent = true;
                            break;
                    }
                }
            }
        }
    }
}
