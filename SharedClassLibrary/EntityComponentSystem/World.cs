using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SharedClassLibrary.ECS
{
    public class World
    {
        public readonly EventBus eventBus;

        private readonly EntityManager entityManager;
        private Dictionary<Type, ComponentManager> componentManagers;

        public World()
        {
            eventBus = new EventBus();
            entityManager = new EntityManager();
            componentManagers = new Dictionary<Type, ComponentManager>();
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EntityHandler CreateEntity()
        {
            Entity newEntity = entityManager.CreateEntity();
            return new EntityHandler(this, newEntity);
        }

        public void RemoveEntity(Entity _entity)
        {
            entityManager.RemoveEntity(_entity);
            foreach (ComponentManager componentManager in componentManagers.Values)
            {
                if (componentManager.EntityHasComponent(_entity)) {
                    componentManager.RemoveComponent(_entity);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AttachComponent<Component>(Entity _entity, Component _component) where Component : struct
        {
            GetComponentManager<Component>().AttachComponent(_entity, _component);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveComponent<Component>(Entity _entity) where Component : struct
        {
            GetComponentManager<Component>().RemoveComponent(_entity);
        }
    }
}
