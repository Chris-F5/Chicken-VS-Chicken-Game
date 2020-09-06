using System.Collections.Generic;

namespace SharedClassLibrary.Simulation
{
    abstract public class System
    {
        private ComponentMask componentMask;
        private List<Entity> concernedEntities = new List<Entity>();

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
                if (!concernedEntities.Contains(_entity))
                {
                    concernedEntities.Add(_entity);
                }
            }
            else
            {
                if (concernedEntities.Contains(_entity))
                {
                    concernedEntities.Remove(_entity);
                }
            }
        }

        abstract public void Update();
    }
}
