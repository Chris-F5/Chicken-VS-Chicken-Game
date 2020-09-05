using System;
using System.Collections.Generic;

namespace SharedClassLibrary.Simulation
{
    struct ComponentMask
    {
        private World world;
        private int setComponentIndex;
        private ComponentManager[] componentManagers;
        private bool active { get { return (setComponentIndex == componentManagers.Length); } }

        public IEnumerable<ComponentManager> ComponentManagers { get { return componentManagers; } }

        public ComponentMask(World _world, int _componentTypeCount)
        {
            world = _world;
            componentManagers = new ComponentManager[_componentTypeCount];
            setComponentIndex = 0;
        }

        public ComponentManager<ComponentType> AddComponentType<ComponentType>() where ComponentType : struct
        {
            if (world != null)
            {
                throw new Exception("Cant add component to ComponentMask when its world field is null.");
            }
            if (setComponentIndex >= componentManagers.Length)
            {
                throw new Exception("Too many component types added to ComponentMask. Please specify a greater component type count in the ComponentMask constructor.");
            }

            ComponentManager<ComponentType> componentManager = world.GetComponentManager<ComponentType>();
            componentManagers[setComponentIndex] = componentManager;
            setComponentIndex++;

            return componentManager;
        }

        public bool FitsEntity(Entity _entity)
        {
            if (!active)
            {
                throw new Exception("Cant check if the mask fits an entity if the mask is not fully set.");
            }

            bool valid = true;

            foreach (ComponentManager componentManager in componentManagers)
            {
                if (!componentManager.EntityHasComponent(_entity))
                {
                    valid = false;
                    break;
                }
            }

            return valid;
        }
    }
}
