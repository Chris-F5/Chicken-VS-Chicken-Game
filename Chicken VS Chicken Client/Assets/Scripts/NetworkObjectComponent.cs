using UnityEngine;
using SharedClassLibrary.Networking;

namespace GameClient
{
    public abstract class NetworkObjectComponent : MonoBehaviour
    {
        public void HandleSynchronise(Packet _packet)
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
                    HandleEvent(_packet, _eventId);
                }
            }
        }
        public virtual void HandleEvent(Packet _packet, byte _eventId) { }
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
