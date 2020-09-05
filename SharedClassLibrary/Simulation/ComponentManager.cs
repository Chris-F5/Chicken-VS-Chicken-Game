using System;
using System.Collections.Generic;

namespace SharedClassLibrary.Simulation
{
    internal abstract class ComponentManager 
    {
        public abstract bool EntityHasComponent(Entity _entity);
        public abstract void SubscribeSystem(System _system);
        internal abstract WorldData.ComponentFamilyData GetEmptyComponentFamilyData();
    }

    internal class ComponentManager<Component> : ComponentManager where Component : struct
    {
        private Dictionary<Entity, Component> components = new Dictionary<Entity, Component>();
        private List<System> subscribedSystems = new List<System>();
        private int maxCount;
        private int id;

        ComponentManager(int _maxCount)
        {
            maxCount = _maxCount;
        }

        internal override WorldData.ComponentFamilyData GetEmptyComponentFamilyData()
        {
            return new WorldData.ComponentFamilyData<Component>(maxCount);
        }

        public override void SubscribeSystem(System _system)
        {
            subscribedSystems.Add(_system);
            foreach (Entity entity in components.Keys)
            {
                _system.UpdateEntityAttachment(entity);
            }
        }

        public void AttachComponent(Entity _entity, Component _component)
        {
            if (components.ContainsKey(_entity))
            {
                throw new Exception("Entity cant have 2 of the same component attached to it.");
            }
            components.Add(_entity, _component);

            foreach (System system in subscribedSystems)
            {
                system.UpdateEntityAttachment(_entity);
            }
        }

        public void RemoveComponent(Entity _entity)
        {
            if (components.ContainsKey(_entity))
            {
                throw new Exception("Entity does not have this component type attached.");
            }
            components.Remove(_entity);

            foreach (System system in subscribedSystems)
            {
                system.UpdateEntityAttachment(_entity);
            }
        }

        public override bool EntityHasComponent(Entity _entity)
        {
            return components.ContainsKey(_entity);
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
