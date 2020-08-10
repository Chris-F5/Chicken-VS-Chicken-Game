using System;
using System.Collections.Generic;

namespace SharedClassLibrary.Simulation
{
    public abstract class Component
    {
        protected readonly NetworkObject networkObject;

        protected Queue<Event> pendingEvents = new Queue<Event>();

        public int pendingEventCount { get { return pendingEvents.Count; } }

        public Component(NetworkObject _networkObject)
        {
            if (_networkObject == null)
                throw new ArgumentNullException("_networkObject is null.");

            networkObject = _networkObject;
            AddStartupEvents();
        }
        public virtual void Update() { }

        public virtual void AddStartupEventsToQueue(ref Queue<Event> _queue)
        {
            //new Event().AddEventToQueue(ref _queue);
        }
        protected void AddStartupEvents()
        {
            AddStartupEventsToQueue(ref pendingEvents);
        }

        public virtual void Dispose()
        {
        }

        public class Event
        {
            public virtual void AddEventToQueue(ref Queue<Event> _queue)
            {
                _queue.Enqueue(this);
            }
        }

        /// <summary>
        /// When this event is added to queue, it will dissable all other events of the same type from the queue.
        /// </summary>
        public class VirtualEvent : Event
        {
            private bool replaced = false;

            public override void AddEventToQueue(ref Queue<Event> _queue)
            {
                foreach (Event _event in _queue)
                {
                    if (_event.GetType() == this.GetType())
                    {
                        (_event as VirtualEvent).replaced = true;
                    }
                }
                base.AddEventToQueue(ref _queue);
            }
        }
    }
}
