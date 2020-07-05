using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameClient
{
    public abstract class NetworkSynchroniser : MonoBehaviour
    {
        public short synchroniserId { get; protected set; }

        public void HandleSynchronise(Packet _packet)
        {
            while (true)
            {
                byte _eventId = _packet.ReadByte();
                if (_eventId == EventIds.eventsEnd)
                {
                    break;
                }
                else
                {
                    HandleEvent(_eventId, _packet);
                }
            }
        }
        protected virtual void HandleEvent(byte _eventId, Packet _packet)
        {
            Debug.Log($"Handling (event id: {_eventId}) update of {gameObject.name}");

            if (_eventId == EventIds.startupEvents)
            {
                return;
            }
            // TODO: Handle destroy event
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
