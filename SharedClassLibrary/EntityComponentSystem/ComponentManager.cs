using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SharedClassLibrary.ECS
{
    public abstract class ComponentManager 
    {
        public abstract bool EntityHasComponent(Entity _entity);
        public abstract void SubscribeSystem(GameSystem _system);
        public abstract void RemoveComponent(Entity _entity);
        public abstract Type GetComponentType();
    }

    public class ComponentManager<ComponentType> : ComponentManager where ComponentType : struct
    {
        private ComponentPool<ComponentType> componentPool = new ComponentPool<ComponentType>();
        private List<GameSystem> subscribedSystems = new List<GameSystem>();

        public ComponentManager()
        {
            componentPool.init();
        }

        public override void SubscribeSystem(GameSystem _system)
        {
            subscribedSystems.Add(_system);
            foreach (Entity entity in componentPool.GetEntities())
            {
                _system.UpdateEntityAttachment(entity);
            }
        }

        public void AttachComponent(Entity _entity, ComponentType _component)
        {
            componentPool.AttachComponent(_entity, _component);

            foreach (GameSystem system in subscribedSystems)
            {
                system.UpdateEntityAttachment(_entity);
            }
        }

        public override void RemoveComponent(Entity _entity)
        {
            componentPool.RemoveComponent(_entity);

            foreach (GameSystem system in subscribedSystems)
            {
                system.UpdateEntityAttachment(_entity);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref ComponentType GetComponent(Entity _entity)
        {
            return ref componentPool.GetComponent(_entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool EntityHasComponent(Entity _entity)
        {
            return componentPool.EntityHasComponent(_entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override Type GetComponentType()
        {
            return typeof(ComponentType);
        }
    }
}
