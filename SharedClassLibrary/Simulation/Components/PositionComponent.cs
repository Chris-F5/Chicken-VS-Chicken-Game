using SharedClassLibrary.Networking;
using System.Collections.Generic;

namespace SharedClassLibrary.Simulation.Components
{
    public class PositionComponent : Component
    {
        private Vector2 position;

        public Vector2 Position 
        { 
            get { return position; } 
            set 
            {
                position = value;
                new SetPositionEvent(this).AddEventToQueue(ref pendingEvents);
            } 
        }

        public PositionComponent(NetworkObject _object, Vector2 _position) : base(_object) 
        {
            position = _position;
        }

        public override void AddStartupEventsToQueue(ref Queue<Event> _queue)
        {
            base.AddStartupEventsToQueue(ref _queue);
            new SetPositionEvent(this).AddEventToQueue(ref _queue);
        }

        public override void Update()
        {
            new SetPositionEvent(this).AddEventToQueue(ref pendingEvents);
        }

        public class SetPositionEvent : VirtualEvent
        {
            public float xPos;
            public float ypos;
            public SetPositionEvent(PositionComponent _positionComponent)
            {
                xPos = _positionComponent.position.x;
                ypos = _positionComponent.position.y;
            }
        }
    }
}
