using System;
using System.Collections.Generic;
using System.Reflection

namespace SharedClassLibrary.Simulation
{
    public class World
    {
        private readonly EntityManager entityManager;
        private Dictionary<Type, ComponentManager> componentManagers = new Dictionary<Type, ComponentManager>();
        private List<System> systems = new List<System>();

        public EntityHandler CreateEntity()
        {
            Entity newEntity = entityManager.CreateEntity();
            return new EntityHandler(this, newEntity);
        }
        public void AttachComponent<Component>(Entity _entity, Component _component) where Component : struct
        {
            GetComponentManager<Component>().AttachComponent(_entity, _component);
        }
        public void RemoveComponent<Component>(Entity _entity) where Component : struct
        {
            GetComponentManager<Component>().RemoveComponent(_entity);
        }
        public void AddSystem(System _system)
        {
            systems.Add(_system);
        }
        public void Update()
        {
            foreach (System system in systems)
            {
                system.Update();
            }
        }

        public ComponentManager<Component> GetComponentManager<Component>() where Component : struct
        {
            Type type = typeof(Component);
            if (!componentManagers.ContainsKey(type))
            {
                componentManagers.Add(type, new ComponentManager<Component>());
            }
            return componentManagers[type] as ComponentManager<Component>;
        }
    }
}
