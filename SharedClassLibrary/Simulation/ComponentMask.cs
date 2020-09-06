﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace SharedClassLibrary.Simulation
{
    public struct ComponentMask
    {
        private ComponentManager[] componentManagers;

        public IEnumerable<ComponentManager> ComponentManagers { get { return componentManagers; } }

        public ComponentMask(World _world, params Type[] _componentTypes)
        {
            componentManagers = new ComponentManager[_componentTypes.Length];
            for (int i = 0; i < _componentTypes.Length; i++)
            {
                // Use reflections to call world.GetComponentManager with a type
                MethodInfo method = typeof(World).GetMethod(nameof(World.GetComponentManager));
                MethodInfo generic = method.MakeGenericMethod(_componentTypes[i]);
                componentManagers[i] = (ComponentManager)generic.Invoke(_world, null);
            }
        }

        public bool FitsEntity(Entity _entity)
        {
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
