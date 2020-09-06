using System.Collections.Generic;

namespace SharedClassLibrary.Simulation
{
    abstract public class System
    {
        private ComponentMask componentMask;
        public List<Entity> activeEntities { get; private set; } = new List<Entity>();

        protected System(ComponentMask _componentMask)
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
