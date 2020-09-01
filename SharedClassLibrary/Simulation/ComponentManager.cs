using System;
using System.Collections.Generic;

namespace SharedClassLibrary.Simulation
{
    internal class ComponentManager<Component>
    {
        private Dictionary<Entity, Component> components = new Dictionary<Entity, Component>();

        public void AddComponent(Entity _entity, Component _component)
        {
            if (components.ContainsKey(_entity))
            {
                throw new Exception("Entity cant have 2 of the same component attached to it.");
            }
            components.Add(_entity, _component);
        }

        public void RemoveComponent(Entity _entity)
        {
            if (components.ContainsKey(_entity))
            {
                throw new Exception("Entity does not have this component type attached.");
            }
            components.Remove(_entity);
        }

        public Component this[Entity _entity]
        {
            get
            {
                return components[_entity];
            }
            set
            {
                components[_entity] = value;
            }
        }
    }
}
