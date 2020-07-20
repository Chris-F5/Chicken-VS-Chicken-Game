﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    abstract class Component
    {
        protected readonly NetworkObject networkObject;

        protected List<Event> pendingEvents = new List<Event>();

        public int pendingEventCount { get { return pendingEvents.Count; } }

        public Component(NetworkObject _networkObject)
        {
            if (_networkObject == null)
                throw new ArgumentNullException("_networkObject is null.");

            networkObject = _networkObject;
            pendingEvents.Add(new StartupEvents(this));
        }
        public virtual void Update() { }

        /// <summary>The packet this generates will be sent via TCP and UDP to all clients when the object is created. When a new client joins this will also be called and sent to only that client.</summary>
        public virtual void AddStartupEventsToPacket(Packet _packet)
        {
            //new ExampleStartupEvent(this).AddEventToPacket(_packet);
        }

        public void AddEventsToPacket(Packet _packet)
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

        public virtual void Dispose()
        {

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
            public SetFloatArrayEvent(byte _eventId, float[] _values) : base(_eventId)
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

        private class StartupEvents : Event
        {
            private readonly Component component;
            public StartupEvents(Component _component) : base(0) // 0 because it wont be used anyway
            {
                component = _component;
            }
            public override void AddEventToPacket(Packet _packet)
            {
                component.AddStartupEventsToPacket(_packet);
            }
        }
    }
}
