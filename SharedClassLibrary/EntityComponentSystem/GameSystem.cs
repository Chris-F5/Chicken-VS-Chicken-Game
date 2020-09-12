using System.Collections.Generic;

namespace SharedClassLibrary.ECS
{
    abstract public class GameSystem
    {
        private ComponentMask componentMask;
        protected ComponentMask ComponentMask { get { return componentMask; } }
        public List<Entity> activeEntities { get; private set; } = new List<Entity>();

        protected GameSystem(ComponentMask _componentMask)
        {
            componentMask = _componentMask;

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
