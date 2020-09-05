using System;
using System.Collections.Generic;
using System.Text;

namespace SharedClassLibrary.Simulation
{
    class ComponentHandler<ComponentType> where ComponentType : struct
    {
        ComponentManager<ComponentType> componentManager;
        Entity entity;
        World world;

        public ComponentHandler(World _world, Entity _entity)
        {
            entity = _entity;
            world = _world;
            componentManager = world.GetComponentManager<ComponentType>();
        }

        public ComponentType componentData 
        {
            get 
            {
                return componentManager[entity];
            }
            set 
            {
                componentManager[entity] = value;
            }
        }
    }
}
