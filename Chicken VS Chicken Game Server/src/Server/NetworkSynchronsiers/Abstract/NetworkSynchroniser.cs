using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    enum SynchroniserType
    {
        player,
        wall,
        testObject
    }

    abstract class NetworkSynchroniser
    {
        private static Dictionary<short, NetworkSynchroniser> allSynchronisers = new Dictionary<short, NetworkSynchroniser>();

        public readonly short synchroniserId;
        public readonly short typeId;

        protected Queue<Event> pendingEvents = new Queue<Event>();

        public NetworkSynchroniser(SynchroniserType _synchroniseType)
        {
            typeId = (byte)_synchroniseType;

            for (short i = 0; i <= short.MaxValue; i++)
            {
                if (!allSynchronisers.ContainsKey(i))
                {
                    synchroniserId = i;
                    break;
                }
            }
            if (synchroniserId == short.MaxValue)
            {
                Console.WriteLine($"CRITICAL WARNING : max NetworkSynchroniser count reached ({short.MaxValue}) reached - ignoring this NetworkSynchroniser");
                return;
            }
            pendingEvents.Enqueue(new StartupEvents(this));
            allSynchronisers.Add(synchroniserId, this);
        }

        /// <summary>The packet this generates will be sent via TCP and UDP to all clients when the object is created. When a new client joins this will also be called and sent to only that client.</summary>
        protected virtual void AddStartupEventsToPacket(Packet _packet)
        {
            _packet.WriteByte(EventIds.startupEvents);
            //new ExampleStartupEvent(this).AddEventToPacket(_packet);
        }

        private void AddEventsToPacket(Packet _packet)
        {
            if (pendingEvents.Count > 0)
            {
                foreach (Event _event in pendingEvents)
                {
                    _event.AddEventToPacket(_packet);
                }
                pendingEvents.Clear();
            }
        }

        public static Packet GenerateSynchronizationPacket()
        {
            Packet _packet = new Packet(ServerPackets.synchronise);
            // TODO: This line causes an exception sometimes because the the all synchronisers dict is changes on antoher thread. Fix it.
            foreach (KeyValuePair<short, NetworkSynchroniser> _synchroniserKeyPair in allSynchronisers)
            {
                _packet.WriteShort(_synchroniserKeyPair.Value.synchroniserId);
                _packet.WriteShort(_synchroniserKeyPair.Value.typeId);
                _synchroniserKeyPair.Value.AddEventsToPacket(_packet);
                _packet.WriteByte(EventIds.eventsEnd);
            }
            return _packet;
        }

        public static Packet GenerateStartupPacket()
        {
            Packet _packet = new Packet(ServerPackets.synchronise);
            foreach (KeyValuePair<short, NetworkSynchroniser> _synchroniserKeyPair in allSynchronisers)
            {
                _packet.WriteShort(_synchroniserKeyPair.Value.synchroniserId);
                _packet.WriteShort(_synchroniserKeyPair.Value.typeId);
                _synchroniserKeyPair.Value.AddStartupEventsToPacket(_packet);
                _packet.WriteByte(EventIds.eventsEnd);
            }

            return _packet;
        }

        private class StartupEvents : Event
        {
            private readonly NetworkSynchroniser synchroniser;
            public StartupEvents(NetworkSynchroniser _synchroniser) : base(EventIds.startupEvents)
            {
                synchroniser = _synchroniser;
            }
            public override void AddEventToPacket(Packet _packet)
            {
                synchroniser.AddStartupEventsToPacket(_packet);
            }
        }

        // TODO: look into turning events into structs if the gc is not keeping up
        protected abstract class Event
        {
            private readonly byte id;
            public Event(byte _eventTypeId)
            {
                id = _eventTypeId;
            }

            public virtual void AddEventToPacket(Packet _packet)
            {
                _packet.WriteByte(id);
            }

            /// <summary>Sends a TCP version of the event to all clients.</summary>
        }

        protected abstract class SetVector2Event : Event
        {
            readonly float x;
            readonly float y;
            public SetVector2Event(byte _eventId, Vector2 _vector) : base(_eventId)
            {
                x = _vector.x;
                y = _vector.y;
            }
            public override void AddEventToPacket(Packet _packet)
            {
                base.AddEventToPacket(_packet);
                _packet.WriteFloat(x);
                _packet.WriteFloat(y);
            }
        }

        protected abstract class SetFloatArrayEvent : Event
        {
            readonly float[] values;
            public SetFloatArrayEvent(byte _eventId, float[] _values) : base (_eventId)
            {
                values = _values;
            }
            public override void AddEventToPacket(Packet _packet)
            {
                base.AddEventToPacket(_packet);
                foreach (float _value in values)
                {
                    _packet.WriteFloat(_value);
                }
            }
        }
    }
}
