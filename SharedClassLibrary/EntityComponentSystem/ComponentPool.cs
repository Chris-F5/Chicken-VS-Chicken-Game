using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SharedClassLibrary.ECS
{
    struct ComponentPool<ComponentType>
    {
        private const int sizeIncreaseStep = 10;
        private const int componentBufferSize = 20;

        private int componentsFilledSize;
        private ComponentType[] components;
        private Dictionary<Entity, int> entityMap;

        public void init()
        {
            componentsFilledSize = 0;
            components = new ComponentType[componentBufferSize];
            entityMap = new Dictionary<Entity, int>();
        }

        public void AttachComponent(Entity _entity, ComponentType _component = default)
        {
            if (entityMap.ContainsKey(_entity))
            {
                throw new Exception("This entity already has this component type attached.");
            }
            if (componentsFilledSize == components.Length)
            {
                Array.Resize(ref components, components.Length + sizeIncreaseStep);
            }
            components[componentsFilledSize] = _component;
            entityMap.Add(_entity, componentsFilledSize);
            componentsFilledSize++;
        }

        public void RemoveComponent(Entity _entity)
        {
            if (!entityMap.ContainsKey(_entity))
            {
                throw new Exception("This entity does not have this component type attached.");
            }
            int index = entityMap[_entity];
            componentsFilledSize -= 1;
            components[index] = components[componentsFilledSize];
            entityMap.Remove(_entity);
        }

        public ref ComponentType GetComponent(Entity _entity)
        {
            if (!entityMap.ContainsKey(_entity))
            {
                throw new Exception("This entity does not have this component type attached.");
            }
            int index = entityMap[_entity];
            return ref components[index];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool EntityHasComponent(Entity _entity)
        {
            return entityMap.ContainsKey(_entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<Entity> GetEntities()
        {
            return entityMap.Keys;
        }
    }
}
