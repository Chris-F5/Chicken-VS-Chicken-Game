using System;
using System.Collections.Generic;

namespace SharedClassLibrary.ECS
{
    abstract public class GameSystem
    {
        protected readonly ComponentMask componentMask;
        protected readonly EventBus eventBus;
        public List<Entity> activeEntities { get; private set; } = new List<Entity>();

        protected GameSystem(World _world, params Type[] _componentTypes)
        {
            eventBus = _world.eventBus;
            componentMask = new ComponentMask(_world, _componentTypes);

            foreach (ComponentManager componentManager in componentMask.ComponentManagers)
            {
                componentManager.SubscribeSystem(this);
            }
        }

        public void UpdateEntityAttachment(Entity _entity)
        {
            if (componentMask.FitsEntity(_entity))
            {
                if (!activeEntities.Contains(_entity))
                {
                    activeEntities.Add(_entity);
                }
            }
            else
            {
                if (activeEntities.Contains(_entity))
                {
                    activeEntities.Remove(_entity);
                }
            }
        }

        abstract public void Update();
    }
}
