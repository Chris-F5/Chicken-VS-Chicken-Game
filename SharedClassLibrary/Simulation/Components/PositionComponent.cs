using SharedClassLibrary.Networking;

namespace SharedClassLibrary.Simulation.Components
{
    class PositionComponent : Component
    {
        public Vector2 position;
        private Vector2 lastUpdatePosition;

        public PositionComponent(NetworkObject _object, Vector2 _position) : base(_object) 
        {
            position = _position;

            lastUpdatePosition = position;
        }

        public override void AddStartupEventsToPacket(Packet _packet)
        {
            base.AddStartupEventsToPacket(_packet);
            new SetPositionEvent(position).AddEventToPacket(_packet);
        }

        public override void Update()
        {
            if (position != lastUpdatePosition)
            {
                pendingEvents.Add(new SetPositionEvent(position));
                lastUpdatePosition = position;
            }
        }

        private class SetPositionEvent : SetVector2Event
        {
            public SetPositionEvent(Vector2 _position) : base(EventIds.PositionComponent.SetPosition, _position) { }
        }
    }
}
