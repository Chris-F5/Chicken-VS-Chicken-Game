using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SharedClassLibrary.ECS
{
    public class EventBus
    {
        private Dictionary<Type, List<Delegate>> subscribers = new Dictionary<Type, List<Delegate>>();

        public void Subscribe<EventData>(Action<EventData> _handler)
        {
            if (!subscribers.ContainsKey(typeof(EventData)))
            {
                subscribers.Add(typeof(EventData), new List<Delegate>());
            }
            subscribers[typeof(EventData)].Add(_handler);
        }

        public void Publish<EventData>(EventData _data)
        {
            if (subscribers.ContainsKey(typeof(EventData)))
            {
                List<Delegate> methodsToCall = subscribers[typeof(EventData)];
                foreach (Delegate method in methodsToCall)
                {
                    (method as Action<EventData>)(_data);
                }
            }
        }
    }
}
