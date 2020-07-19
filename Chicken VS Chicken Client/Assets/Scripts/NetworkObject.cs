using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameClient
{
    class NetworkObject : MonoBehaviour
    {
        [SerializeField]
        public readonly NetworkObjectComponent[] components;
        public short objectId { get; protected set; }
        public bool startupInfoSent { get; private set; } = false;

        public void HandleSynchronise(Packet _packet)
        {
            while (true)
            {
                byte _componentIndex = _packet.ReadByte();
                if (_componentIndex == EventIds.EventEnd)
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
                    components[_componentIndex].HandleSynchronise(_packet);
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
        protected Vector2 ReadVector2Event(Packet _packet)
        {
            float _x = _packet.ReadFloat();
            float _y = _packet.ReadFloat();
            return new Vector2(_x, _y);
        }
        protected float[] ReadFloatArrayEvent(Packet _packet, int _length)
        {
            float[] _array = new float[_length];
            for (int i = 0; i < _length; i++)
            {
                _array[i] = _packet.ReadFloat();
            }
            return _array;
        }
    }
}
